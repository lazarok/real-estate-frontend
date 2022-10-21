using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using RealEstate.Admin.Models;
using RealEstate.Web.Extensions;

namespace RealEstate.Admin.Services;

public enum HttpMethod
{
    GET,
    POST,
    PUT,
    DELETE,
    UPLOAD
}

public interface IHttpService
{
    Task<ApiResponse> GetAsync(string requestUri);
    Task<ApiResponse> PostAsync(string requestUri, object data);
    Task<ApiResponse> PutAsync(string requestUri, object data);
    Task<ApiResponse> UploadAsync(string requestUri, string fileName, Stream content);
    Task<ApiResponse> UploadAsync(string requestUri, List<(string fileName, Stream content)> files);
    Task<ApiResponse> DeleteAsync(string requestUri);

    Task<ApiResponse<T>> GetAsync<T>(string requestUri);
    Task<ApiResponse<T>> PostAsync<T>(string requestUri, object data);
    Task<ApiResponse<T>> PutAsync<T>(string requestUri, object data);
    Task<ApiResponse<T>> UploadAsync<T>(string requestUri, string fileName, Stream content);
    Task<ApiResponse<T>> UploadAsync<T>(string requestUri, List<(string fileName, Stream content)> files);
    Task<ApiResponse<T>> DeleteAsync<T>(string requestUri);
}

public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly IJwtAuthenticationStateProvider _jwtAuthenticationState;

    private static JsonSerializerOptions DefaultJsonSerializerOptions =>
        new JsonSerializerOptions() {PropertyNameCaseInsensitive = true};

    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    public HttpService(HttpClient httpClient, IJwtAuthenticationStateProvider jwtAuthenticationState)
    {
        _httpClient = httpClient;
        _jwtAuthenticationState = jwtAuthenticationState;
    }

    public async Task<ApiResponse> GetAsync(string requestUri)
    {
        return await ExecuteAsync<ApiResponse>(requestUri, HttpMethod.GET);
    }

    public async Task<ApiResponse> PostAsync(string requestUri, object data)
    {
        return await ExecuteAsync<ApiResponse>(requestUri, HttpMethod.POST, data);
    }

    public async Task<ApiResponse> PutAsync(string requestUri, object data)
    {
        return await ExecuteAsync<ApiResponse>(requestUri, HttpMethod.PUT, data);
    }

    public async Task<ApiResponse> UploadAsync(string requestUri, string fileName, Stream content)
    {
        var multiContent = new MultipartFormDataContent();
        multiContent.Add(new StreamContent(content), "file", fileName);

        return await ExecuteAsync<ApiResponse>(requestUri, HttpMethod.UPLOAD, multiContent);
    }

    public async Task<ApiResponse> UploadAsync(string requestUri, List<(string fileName, Stream content)> files)
    {
        var multiContent = new MultipartFormDataContent();

        foreach (var file in files)
        {
            multiContent.Add(new StreamContent(file.Item2), "file", file.Item1);
        }

        return await ExecuteAsync<ApiResponse>(requestUri, HttpMethod.UPLOAD, multiContent);
    }

    public async Task<ApiResponse> DeleteAsync(string requestUri)
    {
        return await ExecuteAsync<ApiResponse>(requestUri, HttpMethod.DELETE);
    }

    public async Task<ApiResponse<T>> GetAsync<T>(string requestUri)
    {
        return await ExecuteAsync<ApiResponse<T>>(requestUri, HttpMethod.GET);
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string requestUri, object data)
    {
        return await ExecuteAsync<ApiResponse<T>>(requestUri, HttpMethod.POST, data);
    }

    public async Task<ApiResponse<T>> PutAsync<T>(string requestUri, object data)
    {
        return await ExecuteAsync<ApiResponse<T>>(requestUri, HttpMethod.PUT, data);
    }

    public async Task<ApiResponse<T>> UploadAsync<T>(string requestUri, string fileName, Stream content)
    {
        var multiContent = new MultipartFormDataContent();
        multiContent.Add(new StreamContent(content), "file", fileName);

        return await ExecuteAsync<ApiResponse<T>>(requestUri, HttpMethod.UPLOAD, multiContent);
    }

    public async Task<ApiResponse<T>> UploadAsync<T>(string requestUri, List<(string fileName, Stream content)> files)
    {
        var multiContent = new MultipartFormDataContent();

        foreach (var file in files)
        {
            multiContent.Add(new StreamContent(file.Item2), "file", file.Item1);
        }

        return await ExecuteAsync<ApiResponse<T>>(requestUri, HttpMethod.UPLOAD, multiContent);
    }

    public async Task<ApiResponse<T>> DeleteAsync<T>(string requestUri)
    {
        return await ExecuteAsync<ApiResponse<T>>(requestUri, HttpMethod.DELETE);
    }

    private async Task<ApiResponse> ExecuteAsync(string requestUri, HttpMethod httpMethod, object data = null)
    {
        try
        {
            var httpResponseMessage = await GetHttpResponseMessage(requestUri, httpMethod, data);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await Deserialize<ApiResponse>(httpResponseMessage);
            }
            else
            {
                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized) // token expired
                {
                    await _jwtAuthenticationState.LogoutAsync();
                }

                var response = new ApiResponse();

                response.SetError(httpResponseMessage.ReasonPhrase,
                    await httpResponseMessage.Content.ReadAsStringAsync());

                return response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.GetAllMessages());

            var response = new ApiResponse();

            response.SetError("Unhandled Error");

            return response;
        }
    }

    private async Task<T> ExecuteAsync<T>(string requestUri, HttpMethod httpMethod, object data = null)
        where T : ApiResponse, new()
    {
        try
        {
            var httpResponseMessage = await GetHttpResponseMessage(requestUri, httpMethod, data);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await Deserialize<T>(httpResponseMessage);
            }

            // var content = await httpResponseMessage.Content.ReadFromJsonAsync<ApiError>();
            // T response = new()
            // {
            //     Error = content
            // };
            
            T response = new();
            response.SetError(httpResponseMessage.ReasonPhrase, await httpResponseMessage.Content.ReadAsStringAsync());

            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized) // token expired
            {
                string parameter = null;

                try
                {
                    parameter = (string) httpResponseMessage.RequestMessage.Headers.Authorization?.Parameter.Clone();
                }
                catch
                {
                }
                
                if (await TryRefreshTokenAsync(parameter))
                {
                    return await ExecuteAsync<T>(requestUri, httpMethod, data);
                }

                await _jwtAuthenticationState.LogoutAsync();
                return response;
            }

            if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
            {
                await _jwtAuthenticationState.LogoutAsync();
                return response;
            }

            return response;
        }
        catch (Exception ex)
        {
            var response = new T();
            response.SetError("UnhandledError", "Unhandled Error");
            Console.WriteLine(ex.GetAllMessages());
            return response;
        }
    }

    private async Task<HttpResponseMessage> GetHttpResponseMessage(string requestUri, HttpMethod httpMethod,
        object? data = null)
    {
        requestUri = $"api/{requestUri}";

        string? dataJson = null;
        if (data != null && httpMethod != HttpMethod.UPLOAD)
        {
            dataJson = JsonSerializer.Serialize(data);
        }

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(
            new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.Name));

        HttpResponseMessage httpResponseMessage;

        switch (httpMethod)
        {
            case HttpMethod.GET:
                httpResponseMessage = await _httpClient.GetAsync(requestUri);
                break;
            case HttpMethod.POST:
                httpResponseMessage = await _httpClient.PostAsync(requestUri,
                    new StringContent(dataJson, Encoding.UTF8, "application/json"));
                break;
            case HttpMethod.PUT:
                httpResponseMessage = await _httpClient.PutAsync(requestUri,
                    new StringContent(dataJson, Encoding.UTF8, "application/json"));
                break;
            case HttpMethod.UPLOAD:
                httpResponseMessage = await _httpClient.PostAsync(requestUri, (HttpContent) data);
                break;
            case HttpMethod.DELETE:
                httpResponseMessage = await _httpClient.DeleteAsync(requestUri);
                break;
            default:
                throw new NotImplementedException($"Utility code not implemented for this Http method {httpMethod}");
        }

        return httpResponseMessage;
    }

    private async Task<T> Deserialize<T>(HttpResponseMessage httpResponse)
    {
        var responseString = await httpResponse.Content.ReadAsStringAsync();

        return !string.IsNullOrEmpty(responseString)
            ? JsonSerializer.Deserialize<T>(responseString, DefaultJsonSerializerOptions)
            : default;
    }

    private async Task<bool> TryRefreshTokenAsync(string requestAuthorization)
    {
        await Semaphore.WaitAsync();

        try
        {
            var token = await _jwtAuthenticationState.GetToken();
            if (token == null)
                return false;

            if (requestAuthorization != null && requestAuthorization != token.AccessToken) // a previous thread refreshed the token
            {
                return true;
            }

            _httpClient.DefaultRequestHeaders.Authorization = null;
            var httpResponseMessage = await _httpClient.PostAsync($"api/auth/refresh-token",
                new StringContent(JsonSerializer.Serialize(token), Encoding.UTF8, "application/json"));

            if (!httpResponseMessage.IsSuccessStatusCode)
                return false;

            var response = await Deserialize<ApiResponse<TokenModel>>(httpResponseMessage);

            if (!response.Success)
                return false;

            await _jwtAuthenticationState.LoginAsync(response.Data);
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            Semaphore.Release();
        }
    }
}