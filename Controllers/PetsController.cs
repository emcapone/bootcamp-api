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
public class PetsController : ControllerBase
{
    public PetsController()
    {
    }

    /// <summary>
    /// Returns all pets
    /// </summary>
    [HttpGet]
    [SwaggerResponse(200, "The pet(s) are returned.", typeof(Pet[]))]
    public ActionResult<List<Pet>> GetAll() =>
        PetService.GetAll();

    /// <summary>
    /// Returns a pet with a given id
    /// </summary>
    [HttpGet("{id}")]
    [SwaggerResponse(200, "The pet is returned.", typeof(Pet))]
    [SwaggerResponse(404, "A pet with the given id does not exist.", typeof(ProblemDetails))]
    public ActionResult<Pet> Get(int id)
    {
        var pet = PetService.Get(id);

        if (pet == null)
            return NotFound();

        return pet;
    }
    
    /// <summary>
    /// Creates a new pet
    /// </summary>
    [HttpPost]
    [SwaggerResponse(201, "A new pet was created.", typeof(Pet))]
    [SwaggerResponse(400, "The request is invalid.", typeof(ProblemDetails))]
    public IActionResult Create(Pet pet)
    {
        PetService.Add(pet);
        return CreatedAtAction(nameof(Create), new { id = pet.Id }, pet);
    }

    
    /// <summary>
    /// Updates a pet with a given id
    /// </summary>
    [HttpPut("{id}")]
    [SwaggerResponse(204, "The pet with the given id was updated.")]
    [SwaggerResponse(400, "The request is invalid.", typeof(ProblemDetails))]
    [SwaggerResponse(404, "A pet with the given id does not exist.", typeof(ProblemDetails))]
    public IActionResult Update(int id, Pet pet)
    {
        if (id != pet.Id)
            return BadRequest();

        var existingPet = PetService.Get(id);
        if (existingPet is null)
            return NotFound();

        PetService.Update(pet);

        return NoContent();
    }
    
    /// <summary>
    /// Deletes a pet with a given id
    /// </summary>
    [HttpDelete("{id}")]
    [SwaggerResponse(200, "The pet with the given id has been deleted.")]
    [SwaggerResponse(400, "The request is invalid.", typeof(ProblemDetails))]
    [SwaggerResponse(404, "A pet with the given id does not exist.", typeof(ProblemDetails))]
    public IActionResult Delete(int id)
    {
        var pet = PetService.Get(id);

        if (pet is null)
            return NotFound();

        PetService.Delete(id);

        return Ok();
    }
}