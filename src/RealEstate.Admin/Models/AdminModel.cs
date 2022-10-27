using RealEstate.Admin.Services;

namespace RealEstate.Admin.Models;

public class AdminModel
{
    public string Sku { get; set; }
    public UserRole Role { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public bool Active { get; set; }
}