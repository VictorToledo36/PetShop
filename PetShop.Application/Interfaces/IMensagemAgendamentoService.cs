using PetShop.Domain.Entities;

namespace PetShop.Application.Interfaces;

public interface IMensagemAgendamentoService
{
    Task<List<MensagemAgendamento>> ObterPorAgendamentoAsync(int agendamentoId);
    Task<MensagemAgendamento> EnviarAsync(int agendamentoId, string applicationUserId, bool enviadaPeloAdmin, string texto);
    Task<Dictionary<int, int>> ContarNaoLidasAsync(IEnumerable<int> agendamentoIds, bool paraAdmin);
    Task MarcarComoLidasAsync(int agendamentoId, bool paraAdmin);
}
