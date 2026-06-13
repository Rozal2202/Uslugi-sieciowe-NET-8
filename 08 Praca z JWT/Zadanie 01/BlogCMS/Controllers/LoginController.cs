using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogCMS.Constants;
using BlogCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BlogCMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class LoginController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public LoginController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost]
    public ActionResult<string> Login([FromBody] LoginModel userLogin)
    {
        LoginModel? user = Authenticate(userLogin);

        if (user is null)
        {
            return NotFound("user not found");
        }

        string token = GenerateToken(user);

        return Ok(token);
    }

    private string GenerateToken(LoginModel user)
    {
        string? jwtKey = _configuration["Jwt:Key"];
        string? jwtIssuer = _configuration["Jwt:Issuer"];
        string? jwtAudience = _configuration["Jwt:Audience"];

        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException("Brak konfiguracji Jwt:Key.");
        }

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)
        );

        SigningCredentials credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );

        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        ];

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static LoginModel? Authenticate(LoginModel userLogin)
    {
        return UserConstants.Users.FirstOrDefault(user =>
            user.Username.Equals(userLogin.Username, StringComparison.OrdinalIgnoreCase)
            && user.Password == userLogin.Password
        );
    }
}