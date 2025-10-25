using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.ViewComponents.UserLayoutViewComponents;

public class _MobileMenuComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}