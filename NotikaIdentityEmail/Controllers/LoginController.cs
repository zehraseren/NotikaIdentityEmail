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

    public LoginController(SignInManager<AppUser> signInManager, EmailContext context)
    {
        _signInManager = signInManager;
        _context = context;
    }

    [HttpGet]
    public IActionResult UserLogin()
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
            ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre giriniz!");
            return View(model);
        }

        return RedirectToAction("EditProfile", "Profile");
    }
}
