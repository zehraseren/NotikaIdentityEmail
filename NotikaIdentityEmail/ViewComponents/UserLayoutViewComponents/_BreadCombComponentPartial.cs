using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.ViewComponents.UserLayoutViewComponents;

public class _BreadCombComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}