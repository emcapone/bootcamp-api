using Dto;
using bootcamp_api.Exceptions;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using bootcamp_api.Services;

namespace bootcamp_api.Controllers
{
    /// <summary>
    /// Handles incoming HTTP requests for bookmarks
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BookmarksController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;

        /// <summary>
        /// BookmarksController constructor
        /// </summary>
        public BookmarksController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        /// <summary>
        /// Returns all of a user's bookmarks
        /// </summary>
        [HttpGet("{user_id}")]
        [ProducesResponseType(typeof(Bookmark[]), StatusCodes.Status200OK)]
        public IActionResult GetAll(int user_id)
        {
            return new OkObjectResult(_bookmarkService.GetAll(user_id));
        }

        /// <summary>
        /// Returns a bookmark with a given id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Bookmark), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            try
            {
                return new ObjectResult(_bookmarkService.Get(id));
            }
            catch (BookmarkNotFoundException)
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        /// Creates a new bookmark
        /// </summary>
        [HttpPost("{user_id}")]
        [ProducesResponseType(typeof(Bookmark), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public IActionResult Create(int user_id, Bookmark bookmark)
        {
            try
            {
                var createdBookmark = _bookmarkService.Add(user_id, bookmark);
                return CreatedAtAction(nameof(Create), new { id = createdBookmark.Id }, createdBookmark);
            }
            catch (DuplicateBookmarkException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
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
            try
            {
                return new OkObjectResult(_bookmarkService.Update(id, bookmark));
            }
            catch (BookmarkNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
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
            try
            {
                _bookmarkService.Delete(id);
                return new NoContentResult();
            }
            catch (BookmarkNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }
    }
}