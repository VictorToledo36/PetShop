namespace PetShop.Application.DTOs;

public enum StatusSlot
{
    Disponivel = 1,
    Pendente = 2,
    Ocupado = 3
}

public class SlotGrade
{
    public DateTime Data { get; set; }
    public TimeOnly Horario { get; set; }
    public StatusSlot Status { get; set; }
}
