using Microsoft.AspNetCore.JsonPatch;
using SampleApp.DTO;
using SampleApp.Models;

namespace SampleApp.Service;

public interface ISuperHeroService
{
    Task<List<SuperHero>> GetAllSuperHeroes();

    ValueTask<SuperHero?> GetSuperHeroById(int id);

    Task<SuperHero> AddSuperHero(AddSuperHeroDTO hero);

    Task UpdateSuperHero(SuperHero hero);
}