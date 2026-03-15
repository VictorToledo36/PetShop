using PetShop.Domain.Entities;

namespace PetShop.Application.Interfaces;

public interface IServicoService
{
    Task<List<Servico>> ObterTodosAsync();
    Task<Servico?> ObterPorIdAsync(int id);
    Task<bool> CriarAsync(Servico servico);
    Task<bool> AtualizarAsync(Servico servico);
    Task<bool> DesativarAsync(int id);
}