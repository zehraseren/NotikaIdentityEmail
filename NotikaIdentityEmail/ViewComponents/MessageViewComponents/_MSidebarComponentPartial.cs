using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using Microsoft.AspNetCore.Identity;

namespace NotikaIdentityEmail.ViewComponents.MessageViewComponents;

public class _MSidebarComponentPartial : ViewComponent
{
    private readonly EmailContext _context;
    private readonly UserManager<AppUser> _userManager;

    public _MSidebarComponentPartial(EmailContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        ViewBag.sendMessageCount = _context.Messages.Where(s => s.SenderEmail == user.Email).Count();
        ViewBag.receiveMessageCount = _context.Messages.Where(s => s.ReceiverEmail == user.Email).Count();

        return View();
    }
}
