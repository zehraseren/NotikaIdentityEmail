using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;

namespace NotikaIdentityEmail.Controllers;
public class ActivationController : Controller
{
    private readonly EmailContext _context;

    public ActivationController(EmailContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult UserActivation()
    {
        return View();
    }

    [HttpPost]
    public IActionResult UserActivation(int userCodeParameter, string email)
    {
        var code = _context.Users.Where(u => u.Email == email).Select(c => c.ActivationCode).FirstOrDefault();
        if (userCodeParameter == code)
        {
            var value = _context.Users.Where(u => u.Email == email).FirstOrDefault();
            value.EmailConfirmed = true;
            _context.SaveChanges();

            return RedirectToAction("UserLogin", "Login");
        }

        return View();
    }
}
