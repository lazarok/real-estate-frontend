using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.Extensions.Localization;
using RealEstate.Shared.Resources;

namespace RealEstate.Admin.Models;

public class ResetPasswordModel
{
    public string NewPassword { get; set; }
    
    [JsonIgnore]
    public string ConfirmNewPassword { get; set; }
}

public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
{
    private readonly IStringLocalizer<GlobalStrings> _localizer;
    
    public ResetPasswordModelValidator(IStringLocalizer<GlobalStrings> localizer)
    {
        _localizer = localizer;
        
        RuleFor(_ => _.NewPassword)
            .NotEmpty().WithMessage($"'{_localizer["New Password"]}' {_localizer["is empty"]}.")
            .MinimumLength(6).WithMessage($"'{_localizer["New Password"]}' {_localizer["minimum length must be"]} 6.")
            .MaximumLength(16).WithMessage($"'{_localizer["New Password"]}' {_localizer["maximum length must be"]} 16.");
        
        RuleFor(_ => _.ConfirmNewPassword)
            .NotEmpty().WithMessage($"'{_localizer["Confirm New Password"]}' {_localizer["is empty"]}.")
            .Must((_, confirmNewPassword) => _.NewPassword == confirmNewPassword).WithMessage(_localizer["Passwords do not match"]);
    }
}