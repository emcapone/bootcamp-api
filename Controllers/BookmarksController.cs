using bootcamp_api.Models;
using bootcamp_api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace bootcamp_api.Controllers;

/// <summary>
/// Handles incoming HTTP requests for bookmarks
/// </summary>
[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("[controller]")]
public class BookmarksController : ControllerBase
{
    /// <summary>
    /// BookmarksController constructor
    /// </summary>
    public BookmarksController()
    {
    }
    
    /// <summary>
    /// Returns all bookmarks
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(Bookmark[]), StatusCodes.Status200OK)]
    public ActionResult<List<Bookmark>> GetAll() =>
        Ok(BookmarkService.GetAll());

    
    /// <summary>
    /// Returns a bookmark with a given id
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Bookmark), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<Bookmark> Get(int id)
    {
        var bookmark = BookmarkService.Get(id);

        if (bookmark == null)
            return NotFound();

        return Ok(bookmark);
    }

    /// <summary>
    /// Creates a new bookmark
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Bookmark), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult Create(Bookmark bookmark)
    {
        BookmarkService.Add(bookmark);
        return CreatedAtAction(nameof(Create), new { id = bookmark.Id }, bookmark);
    }

    /// <summary>
    /// Updates a bookmark with a given id
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Bookmark), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, Bookmark bookmark)
    {
        if (id != bookmark.Id)
            return BadRequest();

        var existingBookmark = BookmarkService.Get(id);
        if (existingBookmark is null)
            return NotFound();

        BookmarkService.Update(bookmark);

        return Ok(bookmark);
    }

    /// <summary>
    /// Deletes a bookmark with a given id
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var bookmark = BookmarkService.Get(id);

        if (bookmark is null)
            return NotFound();

        BookmarkService.Delete(id);

        return NoContent();
    }
}