using BlogCMS.Models;
using BlogCMS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BlogCMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class PostsController : ControllerBase
{
    private readonly IRepository<Post> _postRepository;

    public PostsController(IRepository<Post> postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        IEnumerable<Post> posts = await _postRepository.GetAllAsync();

        return Ok(posts);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        Post? post = await _postRepository.GetByIdAsync(id);

        if (post is null)
        {
            return NotFound(new
            {
                message = $"Nie znaleziono posta o id: {id}."
            });
        }

        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] Post post)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (post.Published == default)
        {
            post.Published = DateTime.UtcNow;
        }

        int newPostId = await _postRepository.AddAsync(post);

        return CreatedAtAction(
            nameof(GetPostById),
            new { id = newPostId },
            post
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePost(int id, [FromBody] Post post)
    {
        if (id != post.Id)
        {
            return BadRequest(new
            {
                message = "Id z adresu URL musi być takie samo jak Id w treści żądania."
            });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        bool updated = await _postRepository.UpdateAsync(post);

        if (!updated)
        {
            return NotFound(new
            {
                message = $"Nie znaleziono posta o id: {id}."
            });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        bool deleted = await _postRepository.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = $"Nie znaleziono posta o id: {id}."
            });
        }

        return NoContent();
    }
}