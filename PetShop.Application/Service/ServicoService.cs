using Microsoft.EntityFrameworkCore;
using PetShop.Application.Interfaces;
using PetShop.Domain.Entities;
using PetShop.Infrastructure.Persistence;

namespace PetShop.Application.Services;

public class ServicoService : IServicoService
{
    private readonly ApplicationDbContext _context;

    public ServicoService(ApplicationDbContext context)
        => _context = context;

    public async Task<List<Servico>> ObterTodosAsync()
        => await _context.Servicos
            .Include(s => s.Precos)
            .Where(s => s.Ativo)
            .OrderBy(s => s.Nome)
            .ToListAsync();

    public async Task<Servico?> ObterPorIdAsync(int id)
        => await _context.Servicos
            .Include(s => s.Precos)
            .FirstOrDefaultAsync(s => s.Id == id && s.Ativo);

    public async Task<bool> CriarAsync(Servico servico)
    {
        servico.DataCriacao = DateTime.UtcNow;
        servico.Ativo = true;
        _context.Servicos.Add(servico);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> AtualizarAsync(Servico servico)
    {
        var existente = await _context.Servicos
            .FirstOrDefaultAsync(s => s.Id == servico.Id && s.Ativo);

        if (existente is null) return false;

        existente.Nome = servico.Nome;
        existente.Descricao = servico.Descricao;

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DesativarAsync(int id)
    {
        var servico = await _context.Servicos
            .FirstOrDefaultAsync(s => s.Id == id && s.Ativo);

        if (servico is null) return false;

        servico.Ativo = false;
        return await _context.SaveChangesAsync() > 0;
    }
}