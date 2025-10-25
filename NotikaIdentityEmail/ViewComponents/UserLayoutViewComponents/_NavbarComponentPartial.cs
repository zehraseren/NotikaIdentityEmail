using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.ViewComponents.UserLayoutViewComponents;

public class _NavbarComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
