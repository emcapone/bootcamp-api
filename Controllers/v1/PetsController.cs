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
    public class PetsController : ControllerBase
    {

        private readonly IPetService _petService;

        /// <summary>
        /// PetsController constructor
        /// </summary>
        public PetsController(IPetService petService)
        {
            _petService = petService;
        }

        /// <summary>
        /// Returns summary details for all of the user's pets
        /// </summary>
        [HttpGet("GetAll/{user_id}")]
        [ProducesResponseType(typeof(PetListItem[]), StatusCodes.Status200OK)]
        public IActionResult GetAll(ApiVersion version, int user_id)
        {
            return new OkObjectResult(_petService.GetAll(version, user_id));
        }

        /// <summary>
        /// Returns a pet with a given id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Pet), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            try
            {
                return new ObjectResult(_petService.Get(id));
            }
            catch (PetNotFoundException)
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        /// Creates a new pet
        /// </summary>
        [HttpPost("{user_id}")]
        [ProducesResponseType(typeof(Pet), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public IActionResult Create(int user_id, Pet pet)
        {
            try
            {
                var createdPet = _petService.Add(user_id, pet);
                return CreatedAtAction(nameof(Create), new { id = createdPet.Id }, createdPet);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }


        /// <summary>
        /// Updates a pet with a given id
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Pet), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, Pet pet)
        {
            try
            {
                return new OkObjectResult(_petService.Update(id, pet));
            }
            catch (PetNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// Deletes a pet with a given id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            try
            {
                _petService.Delete(id);
                return new NoContentResult();
            }
            catch (PetNotFoundException)
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