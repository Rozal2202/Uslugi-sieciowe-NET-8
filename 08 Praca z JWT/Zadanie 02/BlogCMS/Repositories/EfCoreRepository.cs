using BlogCMS.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogCMS.Repositories;

public sealed class EfCoreRepository<T> : IRepository<T> where T : class
{
    private readonly BlogDbContext _context;
    private readonly DbSet<T> _entities;

    public EfCoreRepository(BlogDbContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _entities
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _entities.FindAsync(id);
    }

    public async Task<int> AddAsync(T entity)
    {
        await _entities.AddAsync(entity);
        await _context.SaveChangesAsync();

        object? idValue = typeof(T).GetProperty("Id")?.GetValue(entity);

        return idValue is int id ? id : 0;
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        object? idValue = typeof(T).GetProperty("Id")?.GetValue(entity);

        if (idValue is not int id)
        {
            return false;
        }

        T? existingEntity = await _entities.FindAsync(id);

        if (existingEntity is null)
        {
            return false;
        }

        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        T? entity = await _entities.FindAsync(id);

        if (entity is null)
        {
            return false;
        }

        _entities.Remove(entity);
        await _context.SaveChangesAsync();

        return true;
    }
}