using PetShop.Domain.Enums;

namespace PetShop.Application.DTOs;

public class PetDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Raca { get; set; }
    public decimal? Peso { get; set; }
    public PortePet Porte { get; set; }
    public string? Observacao { get; set; }
}