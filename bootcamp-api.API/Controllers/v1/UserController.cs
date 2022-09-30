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
    /// Handles incoming HTTP requests for pets
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        /// <summary>
        /// UserController constructor
        /// </summary>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Returns a user with a given id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public IActionResult Get(ApiVersion version, int id)
        {
            try
            {
                return new ObjectResult(_userService.Get(version, id));
            }
            catch (UserNotFoundException)
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public IActionResult Create(ApiVersion version, User user)
        {
            try
            {
                var createdUser = _userService.Add(version, user);
                return CreatedAtAction(nameof(Create), new { id = createdUser.Id }, createdUser);
            }
            catch (DuplicateUserException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// Returns a user if credentials are valid
        /// </summary>
        [HttpPost("Auth")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public IActionResult Get(ApiVersion version, Credentials credentials)
        {
            try
            {
                return new ObjectResult(_userService.Authenticate(version, credentials));
            }
            catch (UnauthorizedException)
            {
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// Updates a user with a given id
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public IActionResult Update(ApiVersion version, int id, User user)
        {
            try
            {
                return new OkObjectResult(_userService.Update(version, id, user));
            }
            catch (UserNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (DuplicateUserException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// Deletes a user with a given id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            try
            {
                _userService.Delete(id);
                return new NoContentResult();
            }
            catch (UserNotFoundException)
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