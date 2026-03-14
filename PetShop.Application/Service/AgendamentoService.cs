using Microsoft.EntityFrameworkCore;
using PetShop.Application.Interfaces;
using PetShop.Domain.Entities;
using PetShop.Domain.Enums;
using PetShop.Infrastructure.Persistence;

namespace PetShop.Application.Services;

public class AgendamentoService : IAgendamentoService
{
    private readonly ApplicationDbContext _context;

    public AgendamentoService(ApplicationDbContext context)
        => _context = context;

    // ── Cliente ──────────────────────────────────────────────────────────────

    public async Task<List<Agendamento>> ObterPorUsuarioAsync(string userId)
        => await _context.Agendamentos
            .Include(a => a.Pet)
            .Include(a => a.Servico)
            .Where(a => a.ApplicationUserId == userId && a.Ativo)
            .OrderByDescending(a => a.DataSolicitada)
            .ToListAsync();

    public async Task<bool> SolicitarAgendamentoAsync(Agendamento agendamento)
    {
        agendamento.Status = StatusAgendamento.Pendente;
        agendamento.DataCriacao = DateTime.UtcNow;
        _context.Agendamentos.Add(agendamento);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> CancelarAsync(int id, string userId)
    {
        var ag = await _context.Agendamentos
            .FirstOrDefaultAsync(a => a.Id == id && a.ApplicationUserId == userId);

        // Cliente só pode cancelar agendamentos Pendentes (segurança!)
        if (ag is null || ag.Status != StatusAgendamento.Pendente) 
            return false;

        ag.Status = StatusAgendamento.Cancelado;
        ag.Ativo = false;
        return await _context.SaveChangesAsync() > 0;
    }

    // ── Admin ─────────────────────────────────────────────────────────────────

    public async Task<List<Agendamento>> ObterTodosAsync()
        => await _context.Agendamentos
            .Include(a => a.Pet)
            .Include(a => a.Servico)
            .OrderByDescending(a => a.DataSolicitada)
            .ToListAsync();

    public async Task<List<Agendamento>> ObterPendenteAsync()
        => await _context.Agendamentos
            .Include(a => a.Pet)
            .Include(a => a.Servico)
            .Where(a => a.Status == StatusAgendamento.Pendente)
            .OrderBy(a => a.DataSolicitada)
            .ToListAsync();

    public async Task<bool> ConfirmarAsync(int id)
        => await MudarStatusAsync(id, StatusAgendamento.Confirmado);

    public async Task<bool> ConcluirAsync(int id)
        => await MudarStatusAsync(id, StatusAgendamento.Concluido);

    public async Task<bool> RecusarAsync(int id, string motivo)
    {
        var ag = await _context.Agendamentos.FindAsync(id);
        if (ag is null) return false;

        ag.Status = StatusAgendamento.Recusado;
        ag.MotivoRecusa = motivo;
        ag.DataResposta = DateTime.UtcNow;
        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<bool> MudarStatusAsync(int id, StatusAgendamento novoStatus)
    {
        var ag = await _context.Agendamentos.FindAsync(id);
        if (ag is null) return false;

        ag.Status = novoStatus;
        ag.DataResposta = DateTime.UtcNow;
        return await _context.SaveChangesAsync() > 0;
    }
}