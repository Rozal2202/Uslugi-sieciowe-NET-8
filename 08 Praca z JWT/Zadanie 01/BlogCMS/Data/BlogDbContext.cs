using BlogCMS.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogCMS.Data;

public sealed class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options)
        : base(options)
    {
    }

    public DbSet<Post> Posts => Set<Post>();
}