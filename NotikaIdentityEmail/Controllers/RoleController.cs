using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NotikaIdentityEmail.Models.IdentityModels;

namespace NotikaIdentityEmail.Controllers;
public class RoleController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<IActionResult> RoleList()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return View(roles);
    }

    [HttpGet]
    public IActionResult CreateRole()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(CreateRoleViewModel crvmodel)
    {
        IdentityRole role = new()
        {
            Name = crvmodel.RoleName
        };

        await _roleManager.CreateAsync(role);

        return RedirectToAction("RoleList");
    }

    public async Task<IActionResult> DeleteRole(string id)
    {
        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);
        await _roleManager.DeleteAsync(role);

        return RedirectToAction("RoleList");
    }

    [HttpGet]
    public async Task<IActionResult> UpdateRole(string id)
    {
        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);

        UpdateRoleViewModel model = new()
        {
            RoleId = role.Id,
            RoleName = role.Name,
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRole(UpdateRoleViewModel urvmodel)
    {
        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == urvmodel.RoleId);
        role.Name = urvmodel.RoleName;
        await _roleManager.UpdateAsync(role);

        return RedirectToAction("RoleList");
    }

    [HttpGet]
    public async Task<IActionResult> UserList()
    {
        var users = await _userManager.Users.ToListAsync();

        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> AssignRole(string id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        TempData["userId"] = user.Id;
        var roles = await _roleManager.Roles.ToListAsync();
        var userRoles = await _userManager.GetRolesAsync(user);

        List<RoleAssignViewModel> ravmodel = new();
        foreach (var role in roles)
        {
            RoleAssignViewModel roleModel = new();
            roleModel.RoleId = role.Id;
            roleModel.RoleName = role.Name;
            roleModel.RoleExist = userRoles.Contains(role.Name);
            ravmodel.Add(roleModel);
        }

        return View(ravmodel);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRole(List<RoleAssignViewModel> ravmodel)
    {
        var userId = TempData["userId"].ToString();
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        foreach (var item in ravmodel)
        {
            if (item.RoleExist)
                await _userManager.AddToRoleAsync(user, item.RoleName);
            else
                await _userManager.RemoveFromRoleAsync(user, item.RoleName);
        }

        return RedirectToAction("UserList");
    }
}
