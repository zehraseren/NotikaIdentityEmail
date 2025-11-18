using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using NotikaIdentityEmail.Models.IdentityModels;

namespace NotikaIdentityEmail.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public ProfileController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        var values = await _userManager.FindByNameAsync(User.Identity.Name);

        UserEditViewModel model = new();
        model.Name = values.Name;
        model.Surname = values.Surname;
        model.UserName = values.UserName;
        model.Email = values.Email;
        model.PhoneNumber = values.PhoneNumber;
        model.City = values.City;
        model.ImageUrl = values.ImageUrl;

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile(UserEditViewModel model)
    {
        if (model.Password == model.ConfirmPassword)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.City = model.City;
            user.ImageUrl = model.ImageUrl;
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);

            await _userManager.UpdateAsync(user);
        }

        return View();
    }
}
