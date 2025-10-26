using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;

namespace NotikaIdentityEmail.ViewComponents.MessageViewComponents;

public class _MCategoryListSidebarComponentPartial : ViewComponent
{
    private readonly EmailContext _context;

    public _MCategoryListSidebarComponentPartial(EmailContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var result = _context.Categories.ToList();
        return View(result);
    }
}
