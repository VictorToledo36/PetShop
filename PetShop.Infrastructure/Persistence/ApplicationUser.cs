using Microsoft.AspNetCore.Identity;
using PetShop.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string NomeCompleto { get; set; } = string.Empty;
    public string? CPF { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    public Endereco? Endereco { get; set; }
    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
    public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
    public bool Ativo { get; set; } = true;
}
