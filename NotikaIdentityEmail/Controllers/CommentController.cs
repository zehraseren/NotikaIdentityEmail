using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace NotikaIdentityEmail.Controllers;

public class CommentController : Controller
{
    private readonly EmailContext _context;
    private readonly UserManager<AppUser> _userManager;

    public CommentController(EmailContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult UserComments()
    {
        var userComments = _context.Comments.Include(u => u.AppUser).ToList();

        return View(userComments);
    }

    public IActionResult UserCommentList()
    {
        var commentList = _context.Comments.Include(u => u.AppUser).ToList();

        return View(commentList);
    }

    [HttpGet]
    public PartialViewResult CreateComment()
    {
        return PartialView();
    }

    [HttpPost]
    public async Task<IActionResult> CreateComment(Comment comment)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        comment.AppUserId = user.Id;
        comment.CommentDate = DateTime.Now;
        comment.CommentStatus = "Onay Bekliyor";

        _context.Comments.Add(comment);
        _context.SaveChanges();

        return RedirectToAction("UserCommentList");
    }
}
