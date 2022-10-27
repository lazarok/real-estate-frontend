using Microsoft.AspNetCore.Components;

namespace RealEstate.Admin.Shared;

public class AdminLayoutSetter : ComponentBase
{
    [CascadingParameter]
    public AdminLayout Layout { get; set; }

    [Parameter]
    public RenderFragment ModalSection { get; set; }
    
    
    [Parameter]
    public RenderFragment ToastSection { get; set; }

    protected override void OnInitialized()
    {
        Layout.SetModalSection(ModalSection);
        Layout.SetToastsSection(ToastSection);
    }
}