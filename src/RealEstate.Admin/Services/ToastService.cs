using Microsoft.Extensions.Localization;
using RealEstate.Shared.Resources;

namespace RealEstate.Admin.Services;
public interface IToastService
{
    event Action<ToastModel>? ShowAction;
    void Show(string title = null , string body = null, ToastType type = ToastType.Warning, int delay = 3000);
}

public class ToastService : IToastService
{
    private IStringLocalizer<GlobalStrings> _localizer;

    public ToastService(IStringLocalizer<GlobalStrings> localizer)
    {
        _localizer = localizer;
    }

    public event Action<ToastModel>? ShowAction;
    
    public void Show(string title = null, string body = null, ToastType type = ToastType.Warning, int delay = 3000)
    {
        var t = title;

        if (string.IsNullOrEmpty(title))
        {
            t = type switch
            {
                ToastType.Warning => _localizer["Warning"],
                ToastType.Success => _localizer["Success"],
                ToastType.Danger => _localizer["Danger"],
                _ => _localizer["Warning"]
            };
        }

        ShowAction?.Invoke(new ToastModel()
        {
            Title = t,
            Body = body ?? "",
            Type = type,
            Delay = delay
        });
    }
}

public class ToastModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int Delay { get; set; } = 3000;
    public ToastType Type { get; set; }
}

public enum ToastType
{
    Warning = 1,
    Success = 2,
    Danger = 3
}