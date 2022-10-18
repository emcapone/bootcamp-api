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
    /// Handles incoming HTTP requests for Messages
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MessageController : ControllerBase
    {

        private readonly IMessageService _messageService;

        /// <summary>
        /// MessageController constructor
        /// </summary>
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        /// <summary>
        /// Creates a new message
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Message), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public IActionResult Create(Message message)
        {
            try
            {
                var createdMessage = _messageService.Add(message);
                return CreatedAtAction(nameof(Create), new { id = createdMessage.Id }, createdMessage);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }
    }
}