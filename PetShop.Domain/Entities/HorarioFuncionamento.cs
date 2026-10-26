namespace PetShop.Domain.Entities;

public class HorarioFuncionamento : EntidadeBase
{
    public DayOfWeek DiaSemana { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFim { get; set; }
}
