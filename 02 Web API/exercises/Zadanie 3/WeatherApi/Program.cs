using Microsoft.AspNetCore.Mvc;
using WeatherApi.Models;
using WeatherApi.Options;
using WeatherApi.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<OpenWeatherOptions>(
    builder.Configuration.GetSection(OpenWeatherOptions.SectionName)
);

builder.Services.AddHttpClient<OpenWeatherService>(httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.openweathermap.org/");
    httpClient.Timeout = TimeSpan.FromSeconds(10);
});

WebApplication app = builder.Build();

app.MapGet(
    "/api/weather/{city}",
    async (
        [FromRoute] string city,
        OpenWeatherService weatherService,
        CancellationToken cancellationToken
    ) =>
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return Results.BadRequest(new
            {
                message = "Nazwa miasta jest wymagana."
            });
        }

        WeatherForecastDto? weather = await weatherService.GetWeatherAsync(
            city,
            cancellationToken
        );

        if (weather is null)
        {
            return Results.NotFound(new
            {
                message = $"Nie znaleziono pogody dla miasta: {city}."
            });
        }

        return Results.Ok(weather);
    }
);

app.Run();