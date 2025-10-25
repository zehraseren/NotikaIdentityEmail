using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Entities;
using Microsoft.AspNetCore.Identity;
using NotikaIdentityEmail.Models.IdentityModels;

namespace NotikaIdentityEmail.Controllers;
public class RegisterController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public RegisterController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult CreateUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(RegisterUserViewModel model)
    {
        AppUser appUser = new AppUser()
        {
            Name = model.Name,
            Surname = model.Surname,
            UserName = model.Username,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(appUser, model.Password);

        if (result.Succeeded)
        {
            return RedirectToAction("UserLogin", "User");
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
