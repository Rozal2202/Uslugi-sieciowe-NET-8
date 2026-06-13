using BlogCMS.Constants;
using BlogCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogCMS.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class RegisterController : ControllerBase
{
    [HttpPost]
    public IActionResult Register([FromBody] RegisterUserRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        bool userExists = UserConstants.Users.Any(user =>
            user.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase)
        );

        if (userExists)
        {
            return Conflict(new
            {
                message = $"Użytkownik o nazwie {request.Username} już istnieje."
            });
        }

        LoginModel newUser = new LoginModel
        {
            Username = request.Username,
            Password = request.Password,
            Role = "Admin"
        };

        UserConstants.Users.Add(newUser);

        return Created(string.Empty, new
        {
            username = newUser.Username,
            role = newUser.Role,
            message = "Użytkownik został zarejestrowany."
        });
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = UserConstants.Users.Select(user => new
        {
            username = user.Username,
            role = user.Role
        });

        return Ok(users);
    }
}