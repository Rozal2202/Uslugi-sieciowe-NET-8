using System.Security.Claims;
using BlogCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogCMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class UserController : ControllerBase
{
    [HttpGet]
    [Route("Admins")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminEndPoint()
    {
        LoginModel? currentUser = GetCurrentUser();

        if (currentUser is null)
        {
            return Unauthorized();
        }

        return Ok($"Hi you are an {currentUser.Role}");
    }

    private LoginModel? GetCurrentUser()
    {
        ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;

        if (identity is null)
        {
            return null;
        }

        IEnumerable<Claim> userClaims = identity.Claims;

        return new LoginModel
        {
            Username = userClaims.FirstOrDefault(claim =>
                claim.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty,

            Role = userClaims.FirstOrDefault(claim =>
                claim.Type == ClaimTypes.Role)?.Value ?? string.Empty
        };
    }
}