using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using NotikaIdentityEmail.Models.MessageViewModels;

namespace NotikaIdentityEmail.Controllers;

public class MessageController : Controller
{
    private readonly EmailContext _context;
    private readonly UserManager<AppUser> _userManager;

    public MessageController(EmailContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Inbox()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        var messages = (from m in _context.Messages
                        join u in _context.Users
                        on m.SenderEmail equals u.Email into userGroup
                        from sender in userGroup.DefaultIfEmpty()

                        join c in _context.Categories
                        on m.CategoryId equals c.CategoryId into categoryGroup
                        from category in categoryGroup.DefaultIfEmpty()

                        where m.ReceiverEmail == user.Email
                        select new MessageWithSenderInfoViewModel
                        {
                            MessageId = m.MessageId,
                            SenderEmail = m.SenderEmail,
                            MessageDetail = m.MessageDetail,
                            Subject = m.Subject,
                            SendDate = m.SendDate,
                            SenderName = sender != null ? sender.Name : "Bilinmeyen",
                            SenderSurname = sender != null ? sender.Surname : "Kullanıcı",
                            CategoryName = category != null ? category.CategoryName : "Kategori Yok"
                        }).ToList();

        return View(messages);
    }

    public async Task<IActionResult> Sendbox()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        var messages = (from m in _context.Messages
                        join u in _context.Users
                        on m.ReceiverEmail equals u.Email into userGroup
                        from receiver in userGroup.DefaultIfEmpty()

                        join c in _context.Categories
                        on m.CategoryId equals c.CategoryId into categoryGroup
                        from category in categoryGroup.DefaultIfEmpty()

                        where m.SenderEmail == user.Email
                        select new MessageWithReceiverInfoViewModel
                        {
                            MessageId = m.MessageId,
                            ReceiverEmail = m.ReceiverEmail,
                            MessageDetail = m.MessageDetail,
                            Subject = m.Subject,
                            SendDate = m.SendDate,
                            ReceiverName = receiver != null ? receiver.Name : "Bilinmeyen",
                            ReceiverSurname = receiver != null ? receiver.Surname : "Kullanıcı",
                            CategoryName = category != null ? category.CategoryName : "Kategori Yok"
                        }).ToList();

        return View(messages);
    }

    public IActionResult MessageDetail(int id)
    {
        var messageDetail = _context.Messages.Where(m => m.MessageId == id).FirstOrDefault();
        return View(messageDetail);
    }

    [HttpGet]
    public IActionResult ComposeMessage()
    {
        var categories = _context.Categories.ToList();
        ViewBag.Categories = categories.Select(c => new SelectListItem
        {
            Text = c.CategoryName,
            Value = c.CategoryId.ToString()
        });

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ComposeMessage(Message message)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        message.SendDate = DateTime.Now;
        message.IsRead = false;
        message.SenderEmail = user.Email;

        _context.Messages.Add(message);
        _context.SaveChanges();

        return RedirectToAction("Sendbox");
    }

    public async Task<IActionResult> GetMessageListByCategory(int id)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        var values = (from m in _context.Messages
                      join u in _context.Users
                      on m.SenderEmail equals u.Email into userGroup
                      from sender in userGroup.DefaultIfEmpty()

                      join c in _context.Categories
                      on m.CategoryId equals c.CategoryId into categoryGroup
                      from Category in categoryGroup.DefaultIfEmpty()

                      where m.ReceiverEmail == user.Email && m.CategoryId == id
                      select new MessageWithSenderInfoViewModel
                      {
                          MessageId = m.MessageId,
                          MessageDetail = m.MessageDetail,
                          Subject = m.Subject,
                          SendDate = m.SendDate,
                          SenderEmail = m.SenderEmail,
                          SenderName = sender != null ? sender.Name : "Bilinmeyen",
                          SenderSurname = sender != null ? sender.Surname : "Kullanıcı",
                          CategoryName = Category != null ? Category.CategoryName : "Kategori Yok"
                      }).ToList();

        return View(values);
    }
}
