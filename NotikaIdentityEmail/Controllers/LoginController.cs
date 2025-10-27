using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using Microsoft.AspNetCore.Identity;
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
        var value = _context.Users.Where(u => u.UserName == model.Username).FirstOrDefault();
        if (value.EmailConfirmed)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, true);
            if (result.Succeeded)
            {
                return RedirectToAction("Profile", "MyProfile");
            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
            }
        }
        else
        {
            ModelState.AddModelError("", "Email hesbaınız onaylanmamıştır, lütfen email aktivasyon işlemini gerçekleştiriniz.");
        }

        return View();
    }
}
