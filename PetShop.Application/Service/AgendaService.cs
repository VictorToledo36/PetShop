using Microsoft.EntityFrameworkCore;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Domain.Entities;
using PetShop.Domain.Enums;
using PetShop.Infrastructure.Persistence;

namespace PetShop.Application.Services;

public class AgendaService : IAgendaService
{
    private readonly ApplicationDbContext _context;

    public AgendaService(ApplicationDbContext context)
        => _context = context;

    // ── Horário de funcionamento ────────────────────────────────────────────

    public async Task<List<HorarioFuncionamento>> ObterHorariosAsync()
        => await _context.HorariosFuncionamento
            .OrderBy(h => h.DiaSemana)
            .ToListAsync();

    public async Task SalvarHorariosAsync(List<HorarioFuncionamento> horarios)
    {
        foreach (var horario in horarios)
        {
            var existente = await _context.HorariosFuncionamento
                .FirstOrDefaultAsync(h => h.DiaSemana == horario.DiaSemana);

            if (existente is null)
            {
                _context.HorariosFuncionamento.Add(new HorarioFuncionamento
                {
                    DiaSemana = horario.DiaSemana,
                    HoraInicio = horario.HoraInicio,
                    HoraFim = horario.HoraFim,
                    Ativo = horario.Ativo
                });
            }
            else
            {
                existente.HoraInicio = horario.HoraInicio;
                existente.HoraFim = horario.HoraFim;
                existente.Ativo = horario.Ativo;
            }
        }

        await _context.SaveChangesAsync();
    }

    // ── Bloqueios ────────────────────────────────────────────────────────────

    public async Task<List<BloqueioAgenda>> ObterBloqueiosAsync()
        => await _context.BloqueiosAgenda
            .Where(b => b.Data.Date >= DateTime.Today)
            .OrderBy(b => b.Data)
            .ThenBy(b => b.Horario)
            .ToListAsync();

    public async Task<bool> AdicionarBloqueioAsync(DateTime data, string? motivo, TimeOnly? horario = null)
    {
        _context.BloqueiosAgenda.Add(new BloqueioAgenda { Data = data.Date, Horario = horario, Motivo = motivo });
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoverBloqueioAsync(int id)
    {
        var bloqueio = await _context.BloqueiosAgenda.FindAsync(id);
        if (bloqueio is null) return false;

        _context.BloqueiosAgenda.Remove(bloqueio);
        return await _context.SaveChangesAsync() > 0;
    }

    // ── Configuração ─────────────────────────────────────────────────────────

    public async Task<ConfiguracaoAgenda> ObterConfiguracaoAsync()
    {
        var config = await _context.ConfiguracoesAgenda.FirstOrDefaultAsync();
        if (config is not null) return config;

        config = new ConfiguracaoAgenda();
        _context.ConfiguracoesAgenda.Add(config);
        await _context.SaveChangesAsync();
        return config;
    }

    public async Task AtualizarConfiguracaoAsync(int duracaoSlotMinutos, int diasAntecedenciaMaxima)
    {
        var config = await ObterConfiguracaoAsync();
        config.DuracaoSlotMinutos = duracaoSlotMinutos;
        config.DiasAntecedenciaMaxima = diasAntecedenciaMaxima;
        await _context.SaveChangesAsync();
    }

    // ── Grade de disponibilidade ────────────────────────────────────────────

    public async Task<List<SlotGrade>> ObterGradeAsync(DateTime data)
    {
        var dia = data.Date;
        var config = await ObterConfiguracaoAsync();

        if (dia < DateTime.Today || dia > DateTime.Today.AddDays(config.DiasAntecedenciaMaxima))
            return new List<SlotGrade>();

        var bloqueiosDoDia = await _context.BloqueiosAgenda
            .Where(b => b.Data.Date == dia)
            .ToListAsync();

        // Bloqueio sem horário = dia inteiro bloqueado.
        if (bloqueiosDoDia.Any(b => b.Horario is null))
            return new List<SlotGrade>();

        var horariosBloqueados = bloqueiosDoDia
            .Where(b => b.Horario is not null)
            .Select(b => b.Horario!.Value)
            .ToHashSet();

        var horarioFuncionamento = await _context.HorariosFuncionamento
            .FirstOrDefaultAsync(h => h.DiaSemana == dia.DayOfWeek && h.Ativo);
        if (horarioFuncionamento is null) return new List<SlotGrade>();

        var horarios = GerarHorarios(horarioFuncionamento, config.DuracaoSlotMinutos);

        var agora = TimeOnly.FromDateTime(DateTime.Now);
        if (dia == DateTime.Today)
            horarios = horarios.Where(h => h > agora).ToList();

        if (horarios.Count == 0) return new List<SlotGrade>();

        var ocupados = await _context.Agendamentos
            .Where(a => a.Ativo && a.DataSolicitada.Date == dia &&
                (a.Status == StatusAgendamento.Pendente || a.Status == StatusAgendamento.Confirmado))
            .Select(a => new { a.HorarioSolicitado, a.Status })
            .ToListAsync();

        var ocupadosPorHorario = ocupados.ToDictionary(o => o.HorarioSolicitado, o => o.Status);

        return horarios.Select(h => new SlotGrade
        {
            Data = dia,
            Horario = h,
            Status = horariosBloqueados.Contains(h)
                ? StatusSlot.Ocupado
                : !ocupadosPorHorario.TryGetValue(h, out var status)
                    ? StatusSlot.Disponivel
                    : status == StatusAgendamento.Pendente
                        ? StatusSlot.Pendente
                        : StatusSlot.Ocupado
        }).ToList();
    }

    public async Task<bool> EstaDisponivelAsync(DateTime data, TimeOnly horario)
    {
        var grade = await ObterGradeAsync(data);
        return grade.Any(s => s.Horario == horario && s.Status == StatusSlot.Disponivel);
    }

    private static List<TimeOnly> GerarHorarios(HorarioFuncionamento horario, int duracaoMinutos)
    {
        var inicioMin = horario.HoraInicio.Hour * 60 + horario.HoraInicio.Minute;
        var fimMin = horario.HoraFim.Hour * 60 + horario.HoraFim.Minute;

        var horarios = new List<TimeOnly>();
        for (var m = inicioMin; m + duracaoMinutos <= fimMin; m += duracaoMinutos)
            horarios.Add(new TimeOnly(m / 60, m % 60));

        return horarios;
    }
}
