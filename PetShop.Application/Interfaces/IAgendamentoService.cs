using PetShop.Domain.Entities;
using PetShop.Domain.Enums;

namespace PetShop.Application.Interfaces;

public interface IAgendamentoService
{
    // Cliente
    Task<List<Agendamento>> ObterPorUsuarioAsync(string userId);
    Task<bool> SolicitarAgendamentoAsync(Agendamento agendamento);
    Task<bool> CancelarAsync(int id, string userId);

    // Admin
    Task<List<Agendamento>> ObterTodosAsync();
    Task<List<Agendamento>> ObterPendenteAsync();
    Task<bool> ConfirmarAsync(int id);
    Task<bool> RecusarAsync(int id, string motivo);
    Task<bool> ConcluirAsync(int id);
}