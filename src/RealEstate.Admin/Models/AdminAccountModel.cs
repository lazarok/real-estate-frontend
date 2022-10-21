using RealEstate.Admin.Services;

namespace RealEstate.Admin.Models;

public class AdminAccountModel
{
    public string Sku { get; set; }
    public UserRole Role { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public bool ChangePassword { get; set; }
}