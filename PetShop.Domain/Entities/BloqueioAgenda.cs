namespace PetShop.Domain.Entities;

public class BloqueioAgenda : EntidadeBase
{
    public DateTime Data { get; set; }
    public string? Motivo { get; set; }
}
