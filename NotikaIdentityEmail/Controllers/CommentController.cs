using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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

    [Authorize(Roles = "Admin")]
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

        _context.Comments.Add(comment);
        _context.SaveChanges();

        return RedirectToAction("UserCommentList");
    }

    public IActionResult DeleteComment(int id)
    {
        var comment = _context.Comments.Find(id);

        _context.Comments.Remove(comment);
        _context.SaveChanges();

        return RedirectToAction("UserCommentList");
    }

    public IActionResult CommentStatusChangeToToxic(int id)
    {
        var comment = _context.Comments.Find(id);

        comment.CommentStatus = "Toksik Yorum";
        _context.SaveChanges();

        return RedirectToAction("UserCommentList");
    }

    public IActionResult CommentStatusChangeToPassive(int id)
    {
        var comment = _context.Comments.Find(id);

        comment.CommentStatus = "Yorum Kaldırıldı";
        _context.SaveChanges();

        return RedirectToAction("UserCommentList");
    }

    public IActionResult CommentStatusChangeToActive(int id)
    {
        var comment = _context.Comments.Find(id);

        comment.CommentStatus = "Yorum Onaylandı";
        _context.SaveChanges();

        return RedirectToAction("UserCommentList");
    }
}
