using Microsoft.AspNetCore.Mvc;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi.Controllers;

[ApiController]
[Route("api/cities")]
public sealed class CitiesController : ControllerBase
{
    private readonly CityService _cityService;
    private readonly OpenWeatherService _openWeatherService;

    public CitiesController(
        CityService cityService,
        OpenWeatherService openWeatherService)
    {
        _cityService = cityService;
        _openWeatherService = openWeatherService;
    }

    [HttpGet]
    public ActionResult<IReadOnlyList<CityDto>> GetAll()
    {
        IReadOnlyList<CityDto> cities = _cityService.GetAll();

        return Ok(cities);
    }

    [HttpGet("{id:int}")]
    public ActionResult<CityDto> GetById(int id)
    {
        CityDto? city = _cityService.GetById(id);

        if (city is null)
        {
            return NotFound(new
            {
                message = $"Nie znaleziono miasta o id: {id}."
            });
        }

        return Ok(city);
    }

    [HttpPost]
    public ActionResult<CityDto> Create(CreateCityRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new
            {
                message = "Nazwa miasta jest wymagana."
            });
        }

        if (string.IsNullOrWhiteSpace(request.Country))
        {
            return BadRequest(new
            {
                message = "Kod kraju jest wymagany."
            });
        }

        CityDto createdCity = _cityService.Create(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdCity.Id },
            createdCity
        );
    }

    [HttpPut("{id:int}")]
    public ActionResult<CityDto> Update(int id, UpdateCityRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new
            {
                message = "Nazwa miasta jest wymagana."
            });
        }

        if (string.IsNullOrWhiteSpace(request.Country))
        {
            return BadRequest(new
            {
                message = "Kod kraju jest wymagany."
            });
        }

        CityDto? updatedCity = _cityService.Update(id, request);

        if (updatedCity is null)
        {
            return NotFound(new
            {
                message = $"Nie znaleziono miasta o id: {id}."
            });
        }

        return Ok(updatedCity);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        bool deleted = _cityService.Delete(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = $"Nie znaleziono miasta o id: {id}."
            });
        }

        return NoContent();
    }

    [HttpGet("{id:int}/weather")]
    public async Task<ActionResult<WeatherForecastDto>> GetWeatherByCityId(
        int id,
        CancellationToken cancellationToken)
    {
        CityDto? city = _cityService.GetById(id);

        if (city is null)
        {
            return NotFound(new
            {
                message = $"Nie znaleziono miasta o id: {id}."
            });
        }

        WeatherForecastDto? weather = await _openWeatherService.GetWeatherAsync(
            city.Name,
            cancellationToken
        );

        if (weather is null)
        {
            return NotFound(new
            {
                message = $"Nie znaleziono prognozy pogody dla miasta: {city.Name}."
            });
        }

        return Ok(weather);
    }
}