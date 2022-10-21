using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using RealEstate.Admin.Models;

namespace RealEstate.Admin.Services;

public interface IJwtAuthenticationStateProvider
{
    Task<TokenModel?> GetToken();
    Task LoginAsync(TokenModel tokenModel);
    Task LogoutAsync();
    Task<AuthenticationState> GetAuthenticationStateAsync();
}

public class JwtAuthenticationStateProvider : AuthenticationStateProvider, IJwtAuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;
    private readonly string _tokenKeyName = "_AUTHTOKEN_";

    private AuthenticationState Anonymous => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    public JwtAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<TokenModel>(_tokenKeyName);
        return (token == null || string.IsNullOrEmpty(token.AccessToken)) ? Anonymous : BuildAuthenticationState(token);
    }

    private AuthenticationState BuildAuthenticationState(TokenModel token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token.AccessToken);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.AccessToken);
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims, "jwt")));
    }

    public async Task<TokenModel?> GetToken()
    {
        if (await _localStorage.ContainKeyAsync(_tokenKeyName))
        {
            return await _localStorage.GetItemAsync<TokenModel>(_tokenKeyName);
        }

        return null;
    }

    public async Task LoginAsync(TokenModel token)
    {
        await _localStorage.SetItemAsync(_tokenKeyName, token);
        var authState = BuildAuthenticationState(token);
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(_tokenKeyName);
        _httpClient.DefaultRequestHeaders.Authorization = null;
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
    }
    
}