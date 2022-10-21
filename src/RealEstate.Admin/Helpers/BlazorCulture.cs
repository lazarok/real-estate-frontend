using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace RealEstate.Admin.Helpers;

public class BlazorCulture
{
    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager _navigationManager;

    public BlazorCulture(IJSRuntime jsRuntime, NavigationManager navigationManager)
    {
        _jsRuntime = jsRuntime;
        _navigationManager = navigationManager;
    }

    public async ValueTask<string> SwitchCulture()
    {
        var culture = CultureInfo.CurrentCulture.Name == "en-US" ?  "es-US" : "en-US";

        await _jsRuntime.InvokeVoidAsync("setBlazorCulture", culture);
        
        _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);

        return culture;
    }
       
    public async ValueTask<string?> GetBlazorCulture()
    {
        return await _jsRuntime.InvokeAsync<string>("getBlazorCulture");
    }
}