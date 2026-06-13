using Microsoft.EntityFrameworkCore;
using TravelQuotesApi.Models;

namespace TravelQuotesApi.Data;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Quote> Quotes => Set<Quote>();
}