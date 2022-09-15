using Dto;
using bootcamp_api.Exceptions;
using System;
using AutoMapper;
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
        private readonly IMapper _mapper;

        /// <summary>
        /// BookmarksController constructor
        /// </summary>
        public BookmarksController(IBookmarkService bookmarkService, IMapper mapper)
        {
            _bookmarkService = bookmarkService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all bookmarks
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Bookmark[]), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var bookmarks = _bookmarkService.GetAll();
            var dtos = _mapper.Map<Domain.Bookmark[], Bookmark[]>(bookmarks);
            return new OkObjectResult(dtos);
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
                var bookmark = _bookmarkService.Get(id);
                var dto = _mapper.Map<Domain.Bookmark, Bookmark>(bookmark);
                return new ObjectResult(dto);
            }
            catch (BookmarkNotFoundException)
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        /// Creates a new bookmark
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Bookmark), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public IActionResult Create(Bookmark bookmark)
        {
            try
            {
                var createdBookmark = _bookmarkService.Add(bookmark);
                var location = Url.RouteUrl(new { Action = "Get", Controller = "Post", id = bookmark.Id });

                var dto = _mapper.Map<Domain.Bookmark, Bookmark>(createdBookmark);

                return new CreatedResult(location, dto);
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
                var editedBookmark = _bookmarkService.Update(id, bookmark);
                var dto = _mapper.Map<Domain.Bookmark, Bookmark>(editedBookmark);

                return new OkObjectResult(dto);
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
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }
    }
}