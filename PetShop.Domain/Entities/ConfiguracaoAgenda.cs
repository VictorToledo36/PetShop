namespace PetShop.Domain.Entities;

public class ConfiguracaoAgenda : EntidadeBase
{
    public int DuracaoSlotMinutos { get; set; } = 30;
    public int DiasAntecedenciaMaxima { get; set; } = 30;
}
