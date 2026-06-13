using WeatherWorkerService.Models;

namespace WeatherWorkerService.Mappers;

public static class WeatherMapper
{
    public static Weather MapToWeather(WeatherData weatherData)
    {
        WeatherDescription? description = weatherData.Weather.FirstOrDefault();

        return new Weather
        {
            City = weatherData.City,
            Country = weatherData.System.Country,
            TemperatureC = weatherData.Main.Temperature,
            FeelsLikeC = weatherData.Main.FeelsLike,
            Humidity = weatherData.Main.Humidity,
            Pressure = weatherData.Main.Pressure,
            WindSpeed = weatherData.Wind.Speed,
            Description = description?.Description ?? "Brak opisu",
            CreatedAt = DateTime.UtcNow
        };
    }
}