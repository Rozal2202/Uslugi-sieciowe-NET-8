namespace WeatherApi.Models;

public sealed record UpdateCityRequest(
    string Name,
    string Country
);