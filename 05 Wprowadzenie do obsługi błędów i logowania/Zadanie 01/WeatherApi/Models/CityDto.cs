namespace WeatherApi.Models;

public sealed record CityDto(
    int Id,
    string Name,
    string Country
);