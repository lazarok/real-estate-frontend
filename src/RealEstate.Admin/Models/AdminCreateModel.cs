using FluentValidation;
using Microsoft.Extensions.Localization;
using RealEstate.Shared.Resources;

namespace RealEstate.Admin.Models;

public class AdminCreateModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password{ get; set; }
}

public class AdminCreateModelValidator : AbstractValidator<AdminCreateModel>
{
    private readonly IStringLocalizer<GlobalStrings> _localizer;
    
    public AdminCreateModelValidator(IStringLocalizer<GlobalStrings> localizer)
    {
        _localizer = localizer;
        
        RuleFor(_ => _.FullName)
            .NotEmpty().WithMessage($"'{_localizer["Name"]}' {_localizer["is empty"]}.")
            .MaximumLength(50).WithMessage($"'{_localizer["Name"]}' {_localizer["maximum length must be"]} 50.");

        RuleFor(_ => _.Email)
            .NotEmpty().WithMessage($"'{_localizer["Email"]}' {_localizer["is empty"]}.")
            .MaximumLength(50).WithMessage($"'{_localizer["Email"]}' {_localizer["maximum length must be"]} 50.")
            .EmailAddress().WithMessage($"'{_localizer["Email"]}' {_localizer["requires a valid email format"]}.");

        RuleFor(_ => _.Password)
            .NotEmpty().WithMessage($"'{_localizer["Password"]}' {_localizer["is empty"]}.")
            .MinimumLength(6).WithMessage($"'{_localizer["Password"]}' {_localizer["minimum length must be"]} 6.")
            .MaximumLength(16).WithMessage($"'{_localizer["Password"]}' {_localizer["maximum length must be"]} 16.");
    }
}