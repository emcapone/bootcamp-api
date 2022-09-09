using bootcamp_api.Models;
using bootcamp_api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace bootcamp_api.Controllers;

/// <summary>
/// Handles incoming HTTP requests for pets
/// </summary>
[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("[controller]")]
public class PetsController : ControllerBase
{
    /// <summary>
    /// PetsController constructor
    /// </summary>
    public PetsController()
    {
    }

    /// <summary>
    /// Returns all pets
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(Pet[]), StatusCodes.Status200OK)]
    public ActionResult<List<Pet>> GetAll() =>
        Ok(PetService.GetAll());

    /// <summary>
    /// Returns a pet with a given id
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Pet), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<Pet> Get(int id)
    {
        var pet = PetService.Get(id);

        if (pet == null)
            return NotFound();

        return Ok(pet);
    }
    
    /// <summary>
    /// Creates a new pet
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Pet), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult Create(Pet pet)
    {
        PetService.Add(pet);
        return CreatedAtAction(nameof(Create), new { id = pet.Id }, pet);
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
        if (id != pet.Id)
            return BadRequest();

        var existingPet = PetService.Get(id);
        if (existingPet is null)
            return NotFound();

        PetService.Update(pet);

        return Ok(pet);
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
        var pet = PetService.Get(id);

        if (pet is null)
            return NotFound();

        PetService.Delete(id);

        return NoContent();
    }
}