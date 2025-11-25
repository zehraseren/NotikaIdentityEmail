using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;

namespace NotikaIdentityEmail.ViewComponents.NavbarHeaderViewComponents;

public class _NotificationListOnNavbarComponentPartial : ViewComponent
{
    private readonly EmailContext _context;

    public _NotificationListOnNavbarComponentPartial(EmailContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var notifications = _context.Notifications.ToList();
        return View(notifications);
    }
}
