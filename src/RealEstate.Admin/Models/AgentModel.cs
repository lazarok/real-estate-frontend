using RealEstate.Admin.Services;

namespace RealEstate.Admin.Models;

public class AgentModel
{
    public string Sku { get; set; }
    public UserRole Role { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? WhatsApp { get; set; }
    public string? Web { get; set; }
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public string? Twitter { get; set; }
    public string? LinkedIn { get; set; }
    public bool Active { get; set; }
}