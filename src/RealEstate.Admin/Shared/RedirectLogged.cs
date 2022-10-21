using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace RealEstate.Admin.Shared;

public class RedirectLogged : ComponentBase
{
    [Inject] 
    private NavigationManager _navigationManager { get; set; }
        
    [CascadingParameter]
    private Task<AuthenticationState> _authenticationStateTask { get; set; }

    [Parameter]
    public string RedirectUrl { get; set; }
        
    protected override async Task OnInitializedAsync()
    {
        var user = (await _authenticationStateTask).User;

        if (user.Identity is {IsAuthenticated: true})
        {
            _navigationManager.NavigateTo(RedirectUrl);
        }
    }
}