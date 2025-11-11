using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.Controllers;
public class ErrorPageController : Controller
{
    [Route("Error/404")]
    public IActionResult Page404()
    {
        return View();
    }

    public IActionResult Page401()
    {
        return View();
    }

    public IActionResult Page403()
    {
        return View();
    }

    [Route("Error/{statusCode}")]
    public IActionResult HandleError(int statusCode)
    {
        return statusCode switch
        {
            404 => RedirectToAction("Page404"),
            401 => RedirectToAction("Page401"),
            403 => RedirectToAction("Page403"),
            _ => View(statusCode)
        };
    }
}
