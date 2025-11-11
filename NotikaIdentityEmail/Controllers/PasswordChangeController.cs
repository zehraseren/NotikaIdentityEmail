using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Entities;
using Microsoft.AspNetCore.Identity;
using NotikaIdentityEmail.Models.ForgetPasswordModels;

namespace NotikaIdentityEmail.Controllers;
public class PasswordChangeController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public PasswordChangeController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult ForgetPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel fpvmodel)
    {
        var user = await _userManager.FindByEmailAsync(fpvmodel.Email);
        string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        var passwordResetTokenLink = Url.Action("ResetPassword", "PasswordChange", new
        {
            userId = user.Id,
            token = passwordResetToken,
        }, HttpContext.Request.Scheme);

        MimeMessage message = new();

        MailboxAddress mailboxFrom = new("Notika Admin", "fatmazehraseren@gmail.com");
        message.From.Add(mailboxFrom);

        MailboxAddress mailboxTo = new("User", fpvmodel.Email);
        message.To.Add(mailboxTo);

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = passwordResetTokenLink;
        message.Body = bodyBuilder.ToMessageBody();
        message.Subject = "Şifre Değişikliği Talebi";

        SmtpClient client = new();
        client.Connect("smtp.gmail.com", 587, false);
        client.Authenticate("fatmazehraseren@gmail.com", "chrtcwrigagaqlpq");
        client.Send(message);
        client.Disconnect(true);

        return View();
    }

    [HttpGet]
    public IActionResult ResetPassword(string userId, string token)
    {
        TempData["userId"] = userId;
        TempData["token"] = token;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel rpvmodel)
    {
        var userId = TempData["userId"];
        var token = TempData["token"];

        if (userId == null || token == null) ViewBag.errorMsg = "Hata Oluştu";

        var user = await _userManager.FindByIdAsync(userId.ToString());
        var result = await _userManager.ResetPasswordAsync(user, token.ToString(), rpvmodel.Password);

        if (result.Succeeded) return RedirectToAction("UserLogin", "Login");

        return View();
    }
}
