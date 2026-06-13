using Microsoft.EntityFrameworkCore;
using WeatherWorkerService;
using WeatherWorkerService.Data;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddHttpClient();

builder.Services.AddHostedService<Worker>();

IHost host = builder.Build();

host.Run();