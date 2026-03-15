using Microsoft.EntityFrameworkCore;
using PetShop.Application.Interfaces;
using PetShop.Domain.Entities;
using PetShop.Infrastructure.Persistence;

namespace PetShop.Application.Services;

public class PetService : IPetService
{
    private readonly ApplicationDbContext _context;

    public PetService(ApplicationDbContext context)
        => _context = context;

    public async Task<List<Pet>> ObterPorUsuarioAsync(string userId)
        => await _context.Pets
            .Where(p => p.ApplicationUserId == userId && p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();

    public async Task<Pet?> ObterPorIdAsync(int id, string userId)
        => await _context.Pets
            .FirstOrDefaultAsync(p => p.Id == id
                                   && p.ApplicationUserId == userId
                                   && p.Ativo);

    public async Task<bool> CriarAsync(Pet pet)
    {
        pet.DataCriacao = DateTime.UtcNow;
        pet.Ativo = true;
        _context.Pets.Add(pet);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> AtualizarAsync(Pet pet, string userId)
    {
        // Segurança: garante que o pet pertence ao usuário
        var existente = await _context.Pets
            .FirstOrDefaultAsync(p => p.Id == pet.Id
                                   && p.ApplicationUserId == userId
                                   && p.Ativo);

        if (existente is null) return false;

        existente.Nome = pet.Nome;
        existente.Raca = pet.Raca;
        existente.Peso = pet.Peso;
        existente.Porte = pet.Porte;
        existente.Observacao = pet.Observacao;

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DesativarAsync(int id, string userId)
    {
        // Segurança: garante que o pet pertence ao usuário
        var pet = await _context.Pets
            .FirstOrDefaultAsync(p => p.Id == id
                                   && p.ApplicationUserId == userId
                                   && p.Ativo);

        if (pet is null) return false;

        pet.Ativo = false;
        return await _context.SaveChangesAsync() > 0;
    }
}