using Microsoft.AspNetCore.Mvc;

namespace WeatherApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class TestErrorsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        throw new Exception("Testowy wyjątek");
    }
}