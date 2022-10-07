using bootcamp_api.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bootcamp_api.Services;
using Microsoft.Extensions.Hosting;
using Dto;
using System;

namespace bootcamp_api.Controllers
{

    /// <summary>
    /// Handles incoming HTTP requests for calendar events
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CalendarEventsController : ControllerBase
    {

        private readonly ICalendarEventService _eventService;

        /// <summary>
        /// CalendarEventController constructor
        /// </summary>
        public CalendarEventsController(ICalendarEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Returns all of the user's calendar events
        /// </summary>
        [HttpGet("GetAll/{user_id}")]
        [ProducesResponseType(typeof(CalendarEvent[]), StatusCodes.Status200OK)]
        public IActionResult GetAll(ApiVersion version, int user_id, [FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                return new OkObjectResult(_eventService.Get(version, user_id, month, year));
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// Returns a calendar event with a given id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CalendarEvent), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public IActionResult Get(ApiVersion version, int id)
        {
            try
            {
                return new ObjectResult(_eventService.Get(version, id));
            }
            catch (CalendarEventNotFoundException)
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        /// Creates a new calendar event
        /// </summary>
        [HttpPost("{user_id}")]
        [ProducesResponseType(typeof(CalendarEvent), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public IActionResult Create(ApiVersion version, int user_id, CalendarEvent calendarEvent)
        {
            try
            {
                var createdCalendarEvent = _eventService.Add(version, user_id, calendarEvent);
                return CreatedAtAction(nameof(Create), new { id = createdCalendarEvent.Id }, createdCalendarEvent);
            }
            catch (DuplicateCalendarEventException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }


        /// <summary>
        /// Updates a calendar event with a given id
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CalendarEvent), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public IActionResult Update(ApiVersion version, int id, CalendarEvent calendarEvent)
        {
            try
            {
                return new OkObjectResult(_eventService.Update(version, id, calendarEvent));
            }
            catch (CalendarEventNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// Deletes a calendar event with a given id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            try
            {
                _eventService.Delete(id);
                return new NoContentResult();
            }
            catch (CalendarEventNotFoundException)
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