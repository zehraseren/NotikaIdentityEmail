using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using Microsoft.AspNetCore.Identity;

namespace NotikaIdentityEmail.ViewComponents.UserLayoutViewComponents;

public class _HeaderComponentPartial : ViewComponent
{
    private readonly EmailContext _context;
    private readonly UserManager<AppUser> _userManager;

    public _HeaderComponentPartial(EmailContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var loginUser = await _userManager.FindByNameAsync(User.Identity.Name);
        var userEmails = loginUser.Email;
        var userEmailCount = _context.Messages.Where(m => m.ReceiverEmail == userEmails).Count();
        ViewBag.userEmailCount = userEmailCount;
        ViewBag.notificationCount = _context.Notifications.Count();

        return View();
    }
}