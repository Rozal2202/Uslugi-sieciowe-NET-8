using Serilog;
using WeatherApi.Options;
using WeatherApi.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
);

builder.Services.Configure<OpenWeatherOptions>(
    builder.Configuration.GetSection(OpenWeatherOptions.SectionName)
);

builder.Services.AddHttpClient<OpenWeatherService>(httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.openweathermap.org/");
    httpClient.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddSingleton<CityService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/api/error");
}

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();