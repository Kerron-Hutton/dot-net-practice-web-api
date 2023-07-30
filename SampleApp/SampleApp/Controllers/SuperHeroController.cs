using System.Net.Mime;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleApp.DTO;
using SampleApp.Models;
using SampleApp.Service;

namespace SampleApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuperHeroController : Controller
{
    private readonly ISuperHeroService _superHeroService;

    public SuperHeroController(ISuperHeroService superHeroService)
    {
        _superHeroService = superHeroService;
    }

    // GET
    [HttpGet]
    public async Task<ActionResult<List<SuperHero>>> GetAllSuperHeroes()
    {
        return Ok(
            await _superHeroService.GetAllSuperHeroes()
        );
    }

    // GET
    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<SuperHero>> GetSuperHeroById(int id)
    {
        var hero = await _superHeroService.GetSuperHeroById(id);

        if (hero is null) return NotFound($"User with id: {id} not found");

        return Ok(hero);
    }

    // Post
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<SuperHero>> AddSuperHero(AddSuperHeroDTO hero)
    {
        var createdHero = await _superHeroService.AddSuperHero(hero);

        return CreatedAtAction(nameof(GetSuperHeroById), new { id = createdHero.Id }, createdHero);
    }

    // Post
    [HttpPatch]
    [Route("{id:int}")]
    public async Task<ActionResult<SuperHero>> PatchSuperHero(JsonPatchDocument<SuperHero>? patchDoc, int id)
    {
        if (patchDoc is null) return BadRequest(ModelState);

        var hero = await _superHeroService.GetSuperHeroById(id);

        if (hero is null) return NotFound();

        patchDoc.ApplyTo(hero, ModelState);

        await _superHeroService.UpdateSuperHero(hero);

        return !ModelState.IsValid ? BadRequest(ModelState) : Ok(hero);
    }
}