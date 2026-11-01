using PetShop.Application.DTOs;
using PetShop.Domain.Entities;

namespace PetShop.Application.Interfaces;

public interface IAgendaService
{
    // Horário de funcionamento
    Task<List<HorarioFuncionamento>> ObterHorariosAsync();
    Task SalvarHorariosAsync(List<HorarioFuncionamento> horarios);

    // Bloqueios
    Task<List<BloqueioAgenda>> ObterBloqueiosAsync();
    Task<bool> AdicionarBloqueioAsync(DateTime data, string? motivo, TimeOnly? horario = null);
    Task<bool> RemoverBloqueioAsync(int id);

    // Configuração
    Task<ConfiguracaoAgenda> ObterConfiguracaoAsync();
    Task AtualizarConfiguracaoAsync(int duracaoSlotMinutos, int diasAntecedenciaMaxima);

    // Grade de disponibilidade
    Task<List<SlotGrade>> ObterGradeAsync(DateTime data);
    Task<bool> EstaDisponivelAsync(DateTime data, TimeOnly horario);
}
