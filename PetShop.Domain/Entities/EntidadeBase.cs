namespace PetShop.Domain.Entities;

public abstract class EntidadeBase
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public bool Ativo { get; set; } = true;
}