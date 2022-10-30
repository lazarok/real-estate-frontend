using FluentValidation;
using Microsoft.Extensions.Localization;
using RealEstate.Shared.Resources;

namespace RealEstate.Admin.Models;

public class AdminUpdateModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public bool Active { get; set; }
}

public class AdminUpdateModelValidator : AbstractValidator<AdminUpdateModel>
{
    private readonly IStringLocalizer<GlobalStrings> _localizer;
    
    public AdminUpdateModelValidator(IStringLocalizer<GlobalStrings> localizer)
    {
        _localizer = localizer;
        
        RuleFor(_ => _.FullName)
            .NotEmpty().WithMessage($"'{_localizer["Name"]}' {_localizer["is empty"]}.")
            .MaximumLength(50).WithMessage($"'{_localizer["Name"]}' {_localizer["maximum length must be"]} 50.");

        RuleFor(_ => _.Email)
            .NotEmpty().WithMessage($"'{_localizer["Email"]}' {_localizer["is empty"]}.")
            .MaximumLength(50).WithMessage($"'{_localizer["Email"]}' {_localizer["maximum length must be"]} 50.")
            .EmailAddress().WithMessage($"'{_localizer["Email"]}' {_localizer["requires a valid email format"]}.");
        
        RuleFor(_ => _.Active)
            .NotNull().WithMessage($"'{_localizer["Active"]}' {_localizer["is required"]}.");
    }
}