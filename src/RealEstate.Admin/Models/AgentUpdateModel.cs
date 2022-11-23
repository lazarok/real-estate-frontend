namespace RealEstate.Admin.Models;

public class AgentUpdateModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
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

public class AgentUpdateModelValidator : AbstractValidator<AgentUpdateModel>
{
    private readonly IStringLocalizer<GlobalStrings> _localizer;
    
    public AgentUpdateModelValidator(IStringLocalizer<GlobalStrings> localizer)
    {
        _localizer = localizer;
        
        RuleFor(_ => _.FullName)
            .NotEmpty().WithMessage($"'{_localizer["Name"]}' {_localizer["is empty"]}.")
            .MaximumLength(50).WithMessage($"'{_localizer["Name"]}' {_localizer["maximum length must be"]} 50.");

        RuleFor(_ => _.Email)
            .NotEmpty().WithMessage($"'{_localizer["Email"]}' {_localizer["is empty"]}.")
            .MaximumLength(50).WithMessage($"'{_localizer["Email"]}' {_localizer["maximum length must be"]} 50.")
            .EmailAddress().WithMessage($"'{_localizer["Email"]}' {_localizer["requires a valid email format"]}.");
        
        RuleFor(_ => _.Phone)
            .MaximumLength(15).WithMessage($"'{_localizer["Phone"]}' {_localizer["maximum length must be"]} 15.");
        
        RuleFor(_ => _.Address)
            .MaximumLength(255).WithMessage($"'{_localizer["Address"]}' {_localizer["maximum length must be"]} 255.");
        
        RuleFor(_ => _.WhatsApp)
            .MaximumLength(255).WithMessage($"'{_localizer["WhatsApp"]}' {_localizer["maximum length must be"]} 255.");
        
        RuleFor(_ => _.Web)
            .MaximumLength(255).WithMessage($"'{_localizer["Web"]}' {_localizer["maximum length must be"]} 255.");
        
        RuleFor(_ => _.Facebook)
            .MaximumLength(255).WithMessage($"'{_localizer["Facebook"]}' {_localizer["maximum length must be"]} 255.");
        
        RuleFor(_ => _.Instagram)
            .MaximumLength(255).WithMessage($"'{_localizer["Instagram"]}' {_localizer["maximum length must be"]} 255.");
        
        RuleFor(_ => _.Twitter)
            .MaximumLength(255).WithMessage($"'{_localizer["Twitter"]}' {_localizer["maximum length must be"]} 255.");
       
        RuleFor(_ => _.LinkedIn)
            .MaximumLength(255).WithMessage($"'{_localizer["LinkedIn"]}' {_localizer["maximum length must be"]} 255.");

        
        RuleFor(_ => _.Active)
            .NotNull().WithMessage($"'{_localizer["Active"]}' {_localizer["is required"]}.");
    }
}