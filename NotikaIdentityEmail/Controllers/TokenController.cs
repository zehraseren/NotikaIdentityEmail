using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using NotikaIdentityEmail.Models.JwtModels;

namespace NotikaIdentityEmail.Controllers;
public class TokenController : Controller
{
    private readonly JwtSettingsViewModel _jwtSettingsViewModel;

    public TokenController(IOptions<JwtSettingsViewModel> jwtSettingsViewModel)
    {
        _jwtSettingsViewModel = jwtSettingsViewModel.Value;
    }

    public IActionResult Generate()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Generate(SimpleUserViewModel suvmodel)
    {
        var claim = new[]
        {
            new Claim("name", suvmodel.Name),
            new Claim("surname", suvmodel.Surname),
            new Claim("username", suvmodel.Username),
            new Claim("city", suvmodel.City),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettingsViewModel.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettingsViewModel.Issuer,
            audience: _jwtSettingsViewModel.Audience,
            claims: claim,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettingsViewModel.ExpireMinutes),
            signingCredentials: creds);

        suvmodel.Token = new JwtSecurityTokenHandler().WriteToken(token);

        return View(suvmodel);
    }
}
