using PetShop.Domain.Entities;

namespace PetShop.Application.Interfaces;

public interface IPetService
{
    Task<List<Pet>> ObterPorUsuarioAsync(string userId);
    Task<Pet?> ObterPorIdAsync(int id, string userId);
    Task<bool> CriarAsync(Pet pet);
    Task<bool> AtualizarAsync(Pet pet, string userId);
    Task<bool> DesativarAsync(int id, string userId);
}