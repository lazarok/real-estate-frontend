using Microsoft.AspNetCore.Components;

namespace RealEstate.Admin.Shared;

public class RedirectToLogin : ComponentBase
{
    [Inject] 
    private NavigationManager _navigationManager { get; set; }

    [Parameter]
    public string LoginPage { get; set; }

    protected override void OnInitialized()
    {
        _navigationManager.NavigateTo($"{LoginPage}");
    }
}