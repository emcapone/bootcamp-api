using bootcamp_api.Models;
using bootcamp_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace bootcamp_api.Controllers;

[ApiController]
[Route("[controller]")]
public class BookmarkController : ControllerBase
{
    public BookmarkController()
    {
    }

    [HttpGet]
    public ActionResult<List<Bookmark>> GetAll() =>
        BookmarkService.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Bookmark> Get(int id)
    {
        var bookmark = BookmarkService.Get(id);

        if (bookmark == null)
            return NotFound();

        return bookmark;
    }

    [HttpPost]
    public IActionResult Create(Bookmark bookmark)
    {
        BookmarkService.Add(bookmark);
        return CreatedAtAction(nameof(Create), new { id = bookmark.Id }, bookmark);
    }

    [HttpPut("{id}")]
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

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var bookmark = BookmarkService.Get(id);

        if (bookmark is null)
            return NotFound();

        BookmarkService.Delete(id);

        return NoContent();
    }
}