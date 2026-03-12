using PetShop.Domain.Enums;

namespace PetShop.Domain.Entities;

public class Pet : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;
    public string? Raca { get; set; }
    public decimal? Peso { get; set; }
    public PortePet Porte { get; set; }
    public string? Observacao { get; set; }
    public string ApplicationUserId { get; set; } = string.Empty;
    public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}