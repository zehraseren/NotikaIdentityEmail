using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Entities;
using Microsoft.AspNetCore.Identity;
using NotikaIdentityEmail.Models.IdentityModels;

namespace NotikaIdentityEmail.Controllers;
public class LoginController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;

    public LoginController(SignInManager<AppUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult UserLogin()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UserLogin(UserLoginViewModel model)
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

        return View();
    }
}
