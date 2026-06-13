using Microsoft.EntityFrameworkCore;
using TravelQuotesApi.Data;
using TravelQuotesApi.Interfaces;
using TravelQuotesApi.Models;

namespace TravelQuotesApi.Repositories;

public sealed class QuoteRepository : IRepository<Quote>
{
    private readonly ApplicationDbContext _context;

    public QuoteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Quote>> GetAllAsync()
    {
        return await _context.Quotes
            .AsNoTracking()
            .OrderBy(quote => quote.Id)
            .ToListAsync();
    }

    public async Task<Quote?> GetByIdAsync(int id)
    {
        return await _context.Quotes.FindAsync(id);
    }

    public async Task<Quote> CreateAsync(Quote entity)
    {
        _context.Quotes.Add(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<bool> UpdateAsync(Quote entity)
    {
        bool exists = await _context.Quotes.AnyAsync(quote => quote.Id == entity.Id);

        if (!exists)
        {
            return false;
        }

        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Quote? quote = await _context.Quotes.FindAsync(id);

        if (quote is null)
        {
            return false;
        }

        _context.Quotes.Remove(quote);
        await _context.SaveChangesAsync();

        return true;
    }
}