using Microsoft.AspNetCore.Mvc;
using TravelQuotesApi.Interfaces;
using TravelQuotesApi.Models;

namespace TravelQuotesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class QuotesController : ControllerBase
{
    private readonly IRepository<Quote> _quoteRepository;

    public QuotesController(IRepository<Quote> quoteRepository)
    {
        _quoteRepository = quoteRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Quote>>> GetQuotes()
    {
        IReadOnlyList<Quote> quotes = await _quoteRepository.GetAllAsync();

        return Ok(quotes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Quote>> GetQuote(int id)
    {
        Quote? quote = await _quoteRepository.GetByIdAsync(id);

        if (quote is null)
        {
            return NotFound(new
            {
                message = $"Nie znaleziono cytatu o id: {id}."
            });
        }

        return Ok(quote);
    }

    [HttpPost]
    public async Task<ActionResult<Quote>> PostQuote(Quote quote)
    {
        if (string.IsNullOrWhiteSpace(quote.Author))
        {
            return BadRequest(new
            {
                message = "Autor cytatu jest wymagany."
            });
        }

        if (string.IsNullOrWhiteSpace(quote.Message))
        {
            return BadRequest(new
            {
                message = "Treść cytatu jest wymagana."
            });
        }

        Quote createdQuote = await _quoteRepository.CreateAsync(quote);

        return CreatedAtAction(
            nameof(GetQuote),
            new { id = createdQuote.Id },
            createdQuote
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutQuote(int id, Quote quote)
    {
        if (id != quote.Id)
        {
            return BadRequest(new
            {
                message = "Id z adresu URL musi być takie samo jak Id w treści żądania."
            });
        }

        if (string.IsNullOrWhiteSpace(quote.Author))
        {
            return BadRequest(new
            {
                message = "Autor cytatu jest wymagany."
            });
        }

        if (string.IsNullOrWhiteSpace(quote.Message))
        {
            return BadRequest(new
            {
                message = "Treść cytatu jest wymagana."
            });
        }

        bool updated = await _quoteRepository.UpdateAsync(quote);

        if (!updated)
        {
            return NotFound(new
            {
                message = $"Nie znaleziono cytatu o id: {id}."
            });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteQuote(int id)
    {
        bool deleted = await _quoteRepository.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = $"Nie znaleziono cytatu o id: {id}."
            });
        }

        return NoContent();
    }
}