using PetShop.Domain.Enums;

namespace PetShop.Domain.Entities;

public class ServicoPreco : EntidadeBase
{
    public int ServicoId { get; set; }
    public PortePet Porte { get; set; }
    public decimal Preco { get; set; }
    public Servico Servico { get; set; } = null!;
}