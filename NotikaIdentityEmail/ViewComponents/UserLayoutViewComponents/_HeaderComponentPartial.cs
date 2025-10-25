using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.ViewComponents.UserLayoutViewComponents;

public class _HeaderComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}