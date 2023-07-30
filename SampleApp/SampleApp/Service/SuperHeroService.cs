using Microsoft.EntityFrameworkCore;
using SampleApp.Data;
using SampleApp.DTO;
using SampleApp.Models;

namespace SampleApp.Service;

public class SuperHeroService : ISuperHeroService
{
    private readonly DataContext _context;

    public SuperHeroService(DataContext context)
    {
        _context = context;
    }

    public Task<List<SuperHero>> GetAllSuperHeroes()
    {
        return _context.SuperHeroes.ToListAsync();
    }

    public ValueTask<SuperHero?> GetSuperHeroById(int id)
    {
        return _context.SuperHeroes.FindAsync(id);
    }

    public async Task<SuperHero> AddSuperHero(AddSuperHeroDTO hero)
    {
        var heroToAdd = new SuperHero
        {
            FirstName = hero.FirstName,
            LastName = hero.LastName,
            Place = hero.Place,
            Name = hero.Name
        };

        var createdHero = _context.SuperHeroes.Add(heroToAdd);

        await _context.SaveChangesAsync();

        return createdHero.Entity;
    }

    public async Task UpdateSuperHero(SuperHero hero)
    {
        _context.SuperHeroes.Update(hero);
        await _context.SaveChangesAsync();
    }
}