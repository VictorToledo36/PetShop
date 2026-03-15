using PetShop.Domain.Enums;

namespace PetShop.Application.DTOs;

public class ServicoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public List<ServicoPrecoDto> Precos { get; set; } = new();
}

public class ServicoPrecoDto
{
    public int Id { get; set; }
    public PortePet Porte { get; set; }
    public decimal Preco { get; set; }
}