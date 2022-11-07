using FluentValidation;
using Microsoft.Extensions.Localization;
using RealEstate.Shared.Resources;

namespace RealEstate.Admin.Models;

public class AdminAccountUpdateModel
{
    public string Email { get; set; }
    public string FullName { get; set; }
}

public class AdminAccountUpdateModelValidator : AbstractValidator<AdminAccountUpdateModel>
{
    private readonly IStringLocalizer<GlobalStrings> _localizer;
    
    public AdminAccountUpdateModelValidator(IStringLocalizer<GlobalStrings> localizer)
    {
        _localizer = localizer;
        
        RuleFor(_ => _.FullName)
            .NotEmpty().WithMessage($"'{_localizer["Name"]}' {_localizer["is empty"]}.")
            .MaximumLength(50).WithMessage($"'{_localizer["Name"]}' {_localizer["maximum length must be"]} 50.");

        RuleFor(_ => _.Email)
            .NotEmpty().WithMessage($"'{_localizer["Email"]}' {_localizer["is empty"]}.")
            .MaximumLength(50).WithMessage($"'{_localizer["Email"]}' {_localizer["maximum length must be"]} 50.")
            .EmailAddress().WithMessage($"'{_localizer["Email"]}' {_localizer["requires a valid email format"]}.");
    }
}