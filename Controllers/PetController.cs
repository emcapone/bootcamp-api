using bootcamp_api.Models;
using bootcamp_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace bootcamp_api.Controllers;

[ApiController]
[Route("[controller]")]
public class PetController : ControllerBase
{
    public PetController()
    {
    }

    [HttpGet]
    public ActionResult<List<Pet>> GetAll() =>
        PetService.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Pet> Get(int id)
    {
        var pet = PetService.Get(id);

        if (pet == null)
            return NotFound();

        return pet;
    }

    [HttpPost]
    public IActionResult Create(Pet pet)
    {
        PetService.Add(pet);
        return CreatedAtAction(nameof(Create), new { id = pet.Id }, pet);
    }

    [HttpPut("{id}")]
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

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var pet = PetService.Get(id);

        if (pet is null)
            return NotFound();

        PetService.Delete(id);

        return NoContent();
    }
}