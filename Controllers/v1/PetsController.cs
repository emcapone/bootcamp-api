using bootcamp_api.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
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
        private readonly IMapper _mapper;

        /// <summary>
        /// PetsController constructor
        /// </summary>
        public PetsController(IPetService petService, IMapper mapper)
        {
            _petService = petService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all pets
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Pet[]), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var pets = _petService.GetAll();
            var dtos = _mapper.Map<Domain.Pet[], Pet[]>(pets);
            return new OkObjectResult(dtos);
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
                var pet = _petService.Get(id);
                var dto = _mapper.Map<Domain.Pet, Pet>(pet);
                return new ObjectResult(dto);
            }
            catch (PetNotFoundException)
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        /// Creates a new pet
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Pet), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public IActionResult Create(Pet pet)
        {
            try
            {
                var createdPet = _petService.Add(pet);
                var location = Url.RouteUrl(new { Action = "Get", Controller = "Post", id = pet.Id });

                var dto = _mapper.Map<Domain.Pet, Pet>(createdPet);

                return new CreatedResult(location, dto);
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
                var editedPet = _petService.Update(id, pet);
                var dto = _mapper.Map<Domain.Pet, Pet>(editedPet);

                return new OkObjectResult(dto);
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
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }
    }
}