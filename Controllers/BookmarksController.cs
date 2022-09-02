using bootcamp_api.Models;
using bootcamp_api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace bootcamp_api.Controllers;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[SwaggerResponse(500, "An error occurred while processing the request", typeof(ProblemDetails))]
[Route("[controller]")]
public class BookmarksController : ControllerBase
{
    public BookmarksController()
    {
    }
    
    /// <summary>
    /// Returns all bookmarks
    /// </summary>
    [HttpGet]
    [SwaggerResponse(200, "The bookmark(s) are returned.", typeof(Bookmark[]))]
    public ActionResult<List<Bookmark>> GetAll() =>
        BookmarkService.GetAll();

    
    /// <summary>
    /// Returns a bookmark with a given id
    /// </summary>
    [HttpGet("{id}")]
    [SwaggerResponse(200, "The bookmark is returned.", typeof(Bookmark))]
    [SwaggerResponse(404, "A bookmark with the given id does not exist.", typeof(ProblemDetails))]
    public ActionResult<Bookmark> Get(int id)
    {
        var bookmark = BookmarkService.Get(id);

        if (bookmark == null)
            return NotFound();

        return bookmark;
    }

    /// <summary>
    /// Creates a new bookmark
    /// </summary>
    [HttpPost]
    [SwaggerResponse(201, "A new bookmark was created.", typeof(Bookmark))]
    [SwaggerResponse(400, "The request is invalid.", typeof(ProblemDetails))]
    public IActionResult Create(Bookmark bookmark)
    {
        BookmarkService.Add(bookmark);
        return CreatedAtAction(nameof(Create), new { id = bookmark.Id }, bookmark);
    }

    /// <summary>
    /// Updates a bookmark with a given id
    /// </summary>
    [HttpPut("{id}")]
    [SwaggerResponse(204, "The bookmark with the given id was updated.")]
    [SwaggerResponse(400, "The request is invalid.", typeof(ProblemDetails))]
    [SwaggerResponse(404, "A bookmark with the given id does not exist.", typeof(ProblemDetails))]
    public IActionResult Update(int id, Bookmark bookmark)
    {
        if (id != bookmark.Id)
            return BadRequest();

        var existingBookmark = BookmarkService.Get(id);
        if (existingBookmark is null)
            return NotFound();

        BookmarkService.Update(bookmark);

        return NoContent();
    }

    /// <summary>
    /// Deletes a bookmark with a given id
    /// </summary>
    [HttpDelete("{id}")]
    [SwaggerResponse(200, "The bookmark with the given id has been deleted.")]
    [SwaggerResponse(400, "The request is invalid.", typeof(ProblemDetails))]
    [SwaggerResponse(404, "A bookmark with the given id does not exist.", typeof(ProblemDetails))]
    public IActionResult Delete(int id)
    {
        var bookmark = BookmarkService.Get(id);

        if (bookmark is null)
            return NotFound();

        BookmarkService.Delete(id);

        return Ok();
    }
}