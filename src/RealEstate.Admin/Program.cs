using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using RealEstate.Admin;
using RealEstate.Admin.Helpers;
using RealEstate.Admin.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var http = new HttpClient()
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
};

if (builder.HostEnvironment.BaseAddress.Contains("dev"))
{
    var response = await http.GetAsync("appsettings.dev.json");
    await using var stream = await response.Content.ReadAsStreamAsync();
    builder.Configuration.AddJsonStream(stream);
}

builder.Services.ConfigureServices(builder.Configuration);

#if DEBUG
    builder.Services.AddSassCompiler();
#endif

var host = builder.Build();
            
await host.SetDefaultCulture();

await host.RunAsync();

public static class Extensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpService(configuration["App:ApiUrl"]);

        services.AddLocalization();

        services.AddScoped<BlazorCulture>();
            
        services.AddOptions();
        services.AddAuthorizationCore();

        services.AddJwtAuthentication();

        services.AddBlazoredLocalStorage();

        services.AddTransient<ICurrentAccountService, CurrentAccountService>();
        services.AddSingleton<IToastService, ToastService>();
        services.AddScoped<TempData>();
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddScoped<JwtAuthenticationStateProvider>();
        services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>(
            provider => provider.GetRequiredService<JwtAuthenticationStateProvider>()
        );
        services.AddScoped<IJwtAuthenticationStateProvider, JwtAuthenticationStateProvider>(
            provider => provider.GetRequiredService<JwtAuthenticationStateProvider>()
        );

        return services;
    }

    private static IServiceCollection AddHttpService(this IServiceCollection services, string baseAddress)
    {
        services.AddSingleton(new HttpClient() { BaseAddress = new Uri(baseAddress), Timeout = TimeSpan.FromHours(1) });
        services.AddScoped<IHttpService, HttpService>();

        return services;
    }
    
    public static async Task SetDefaultCulture(this WebAssemblyHost host)
    {
        var blazorCulture = host.Services.GetRequiredService<BlazorCulture>();
        
        var jsRuntime = host.Services.GetRequiredService<IJSRuntime>();
        var browserLanguage = await jsRuntime.InvokeAsync<string>("getBrowserLanguage");

        var result = await blazorCulture.GetBlazorCulture() ?? browserLanguage;

        var culture = result != null ? new CultureInfo(result) : new CultureInfo("en"); 
        
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}