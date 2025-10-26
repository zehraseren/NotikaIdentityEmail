using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;

namespace NotikaIdentityEmail.Controllers;
public class MessageController : Controller
{
    private readonly EmailContext _context;

    public MessageController(EmailContext context)
    {
        _context = context;
    }

    public IActionResult Inbox()
    {
        var messages = _context.Messages.Where(m => m.ReceiverEmail == "ayse.yilmaz@example.com").ToList();
        return View(messages);
    }

    public IActionResult Sendbox()
    {
        var messages = _context.Messages.Where(m => m.SenderEmail == "info@kampanya365.com").ToList();
        return View(messages);
    }

    public IActionResult MessageDetail()
    {
        var messageDetail = _context.Messages.Where(m => m.MessageId == 7).FirstOrDefault();
        return View(messageDetail);
    }

    [HttpGet]
    public IActionResult ComposeMessage()
    {
        return View();
    }
}
