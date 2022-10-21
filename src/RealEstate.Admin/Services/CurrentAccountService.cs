using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RealEstate.Admin.Services;
public interface ICurrentAccountService
{
    Task<string> GetFullName();
    Task<string> GetEmail();
    Task<bool> IsSuperAdminAsync();
    Task<bool> IsAdminAsync();
    Task<bool> IsAgentAsync();
}

public class CurrentAccountService : ICurrentAccountService
{
    private readonly IJwtAuthenticationStateProvider _jwtAuthenticationStateProvider;

    public CurrentAccountService(IJwtAuthenticationStateProvider jwtAuthenticationStateProvider)
    {
        _jwtAuthenticationStateProvider = jwtAuthenticationStateProvider;
    }
    
    public async Task<string> GetEmail()
    {
        var handler = new JwtSecurityTokenHandler();
        var token = await _jwtAuthenticationStateProvider.GetToken();
        var jwtToken = handler.ReadJwtToken(token.AccessToken);
        var email = jwtToken.Claims.SingleOrDefault(_ => _.Type == ClaimTypes.Email);
        return email?.Value;
    }
    
    public async Task<string> GetFullName()
    {
        var handler = new JwtSecurityTokenHandler();
        var token = await _jwtAuthenticationStateProvider.GetToken();
        var jwtToken = handler.ReadJwtToken(token.AccessToken);
        var name = jwtToken.Claims.SingleOrDefault(_ => _.Type == ClaimTypes.Name);
        return name?.Value;
    }
    
    public async Task<bool> IsSuperAdminAsync()
    {
        var authenticationState = await _jwtAuthenticationStateProvider.GetAuthenticationStateAsync();
        return authenticationState.User.IsInRole(UserRole.SuperAdmin.ToString());
    }

    public async Task<bool> IsAdminAsync()
    {
        var authenticationState = await _jwtAuthenticationStateProvider.GetAuthenticationStateAsync();
        return authenticationState.User.IsInRole(UserRole.Admin.ToString());
    }
    
    public async Task<bool> IsAgentAsync()
    {
        var authenticationState = await _jwtAuthenticationStateProvider.GetAuthenticationStateAsync();
        return authenticationState.User.IsInRole(UserRole.Agent.ToString());
    }
}

// remove to user model
public enum UserRole
{
    Agent = 1,
    Admin = 2,
    SuperAdmin = 3
}