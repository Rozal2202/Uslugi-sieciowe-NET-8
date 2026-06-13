using Microsoft.EntityFrameworkCore;
using WeatherWorkerService.Models;

namespace WeatherWorkerService.Data;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Weather> WeatherRecords => Set<Weather>();
}