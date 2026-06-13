namespace WeatherApi.Models;

public sealed record WeatherForecastDto(
    string City,
    string Country,
    double TemperatureC,
    double FeelsLikeC,
    int Humidity,
    int Pressure,
    double WindSpeed,
    string Description
);