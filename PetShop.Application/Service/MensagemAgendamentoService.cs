using Microsoft.EntityFrameworkCore;
using PetShop.Application.Interfaces;
using PetShop.Domain.Entities;
using PetShop.Infrastructure.Persistence;

namespace PetShop.Application.Services;

public class MensagemAgendamentoService : IMensagemAgendamentoService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public MensagemAgendamentoService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<MensagemAgendamento>> ObterPorAgendamentoAsync(int agendamentoId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.MensagensAgendamento
            .Where(m => m.AgendamentoId == agendamentoId)
            .OrderBy(m => m.DataCriacao)
            .ToListAsync();
    }

    public async Task<MensagemAgendamento> EnviarAsync(int agendamentoId, string applicationUserId, bool enviadaPeloAdmin, string texto)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var mensagem = new MensagemAgendamento
        {
            AgendamentoId = agendamentoId,
            ApplicationUserId = applicationUserId,
            EnviadaPeloAdmin = enviadaPeloAdmin,
            Texto = texto.Trim(),
            Lida = false
        };

        context.MensagensAgendamento.Add(mensagem);
        await context.SaveChangesAsync();
        return mensagem;
    }

    public async Task<Dictionary<int, int>> ContarNaoLidasAsync(IEnumerable<int> agendamentoIds, bool paraAdmin)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.MensagensAgendamento
            .Where(m => agendamentoIds.Contains(m.AgendamentoId) && !m.Lida && m.EnviadaPeloAdmin != paraAdmin)
            .GroupBy(m => m.AgendamentoId)
            .Select(g => new { AgendamentoId = g.Key, Quantidade = g.Count() })
            .ToDictionaryAsync(x => x.AgendamentoId, x => x.Quantidade);
    }

    public async Task MarcarComoLidasAsync(int agendamentoId, bool paraAdmin)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var naoLidas = await context.MensagensAgendamento
            .Where(m => m.AgendamentoId == agendamentoId && !m.Lida && m.EnviadaPeloAdmin != paraAdmin)
            .ToListAsync();

        if (naoLidas.Count == 0) return;

        foreach (var m in naoLidas)
            m.Lida = true;

        await context.SaveChangesAsync();
    }
}
