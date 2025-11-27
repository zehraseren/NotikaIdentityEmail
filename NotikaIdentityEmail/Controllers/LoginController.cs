using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NotikaIdentityEmail.Models.IdentityModels;

namespace NotikaIdentityEmail.Controllers;

public class LoginController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly EmailContext _context;
    private readonly UserManager<AppUser> _userManager;

    public LoginController(SignInManager<AppUser> signInManager, EmailContext context, UserManager<AppUser> userManager)
    {
        _signInManager = signInManager;
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult UserLogin()
    {
        return View();
    }

    [HttpGet]
    public IActionResult LoginWithGoogle()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UserLogin(UserLoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.Username);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı!");
            return View(model);
        }

        if (!user.EmailConfirmed)
        {
            ModelState.AddModelError(string.Empty, "Email hesabınız onaylanmamıştır, lütfen aktivasyon işlemini gerçekleştiriniz.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, true);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Doğru kullanıcı adı veya şifre giriniz!");
            return View(model);
        }

        return RedirectToAction("EditProfile", "Profile");
    }

    [HttpPost]
    public IActionResult ExternalLogin(string provider, string? returnUrl = null)
    {
        var redirectUrl = Url.Action("ExternalLoginCallBack", "Login", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);

        return Challenge(properties, provider);
    }

    [HttpPost]
    public async Task<IActionResult> ExternalLoginCallBack(string? returnUrl = null, string? remoteError = null)
    {
        returnUrl ??= Url.Content("~/");
        if (remoteError != null)
        {
            ModelState.AddModelError(string.Empty, $"External Provider Error: {remoteError}");
            return RedirectToAction("UserLogin");
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return RedirectToAction("UserLogin");
        }

        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
        if (result.Succeeded)
        {
            return RedirectToAction("Inbox", "Message");
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var user = new AppUser
        {
            UserName = email,
            Email = email,
            Name = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "Google",
            Surname = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "User",
        };

        var identityResult = await _userManager.CreateAsync(user);
        if (identityResult.Succeeded)
        {
            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Inbox", "Message");
        }

        return RedirectToAction("UserLogin");
    }
}