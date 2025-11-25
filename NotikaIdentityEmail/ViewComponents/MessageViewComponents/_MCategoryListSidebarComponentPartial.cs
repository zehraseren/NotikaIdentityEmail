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

        var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

        var messageForCategory = (from c in _context.Categories
                                  join m in _context.Messages
                                  on c.CategoryId equals m.CategoryId into msgGroup
                                  select new
                                  {
                                      CategoryId = c.CategoryId,
                                      CategoryIconUrl = c.CategoryIconUrl,
                                      CategoryName = c.CategoryName,
                                      MessageCount = msgGroup.Where(e => e.ReceiverEmail == user.Email).Count()
                                  }).ToList();
        ViewBag.messageForCategory = messageForCategory;

        return View(result);
    }
}
