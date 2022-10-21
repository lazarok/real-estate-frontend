using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.Extensions.Localization;
using RealEstate.Shared.Resources;

namespace RealEstate.Admin.Models;

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginModelValidator : AbstractValidator<LoginModel>
{
    private readonly IStringLocalizer<GlobalStrings> _localizer;
    
    public LoginModelValidator(IStringLocalizer<GlobalStrings> localizer)
    {
        _localizer = localizer;

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