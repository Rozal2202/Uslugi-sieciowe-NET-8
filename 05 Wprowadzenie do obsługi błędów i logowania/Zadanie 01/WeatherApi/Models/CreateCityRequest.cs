namespace WeatherApi.Models;

public sealed record CreateCityRequest(
    string Name,
    string Country
);