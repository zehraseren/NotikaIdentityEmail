using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using NotikaIdentityEmail.Models.IdentityModels;

namespace NotikaIdentityEmail.Controllers;
public class RegisterController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMemoryCache _cache;

    public RegisterController(UserManager<AppUser> userManager, IMemoryCache cache)
    {
        _userManager = userManager;
        _cache = cache;
    }

    [HttpGet]
    public IActionResult CreateUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(RegisterUserViewModel model)
    {
        Random random = new Random();
        int code = random.Next(100000, 1000000);

        AppUser appUser = new AppUser()
        {
            Name = model.Name,
            Surname = model.Surname,
            UserName = model.Username,
            Email = model.Email,
            ActivationCode = code,
        };

        var result = await _userManager.CreateAsync(appUser, model.Password);

        if (result.Succeeded)
        {
            MimeMessage mimeMessage = new MimeMessage();

            // Sender
            MailboxAddress mailboxAddressFrom = new MailboxAddress("Admin", "fatmazehraseren@gmail.com");
            mimeMessage.From.Add(mailboxAddressFrom);

            // Receiver
            MailboxAddress mailboxAddressTo = new MailboxAddress("User", model.Email);
            mimeMessage.To.Add(mailboxAddressTo);

            // Content
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = $"Hesabınızı doğrulamak için gerekli olan aktivasyon kodu: {code}";
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            // Subject
            mimeMessage.Subject = "Notika Identity Aktivasyon Kodu";

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Connect("smtp.gmail.com", 587, false);
            smtpClient.Authenticate("fatmazehraseren@gmail.com", "oillfoqauhrvgcxy");
            smtpClient.Send(mimeMessage);
            smtpClient.Disconnect(true);

            _cache.Set(model.Email, TimeSpan.FromMinutes(5));

            return RedirectToAction("UserActivation", "Activation", new { email = model.Email });
        }
        else
        {
            foreach (var item in result.Errors)
            {
                // "" ifadesi boş bırakarak tüm hataları ekrana yazdırılır
                ModelState.AddModelError("", item.Description);
            }
        }

        return View();
    }
}
