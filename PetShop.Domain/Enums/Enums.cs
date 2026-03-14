namespace PetShop.Domain.Enums;

public enum StatusAgendamento
{
    Pendente = 1,
    Confirmado = 2,
    Recusado = 3,
    Cancelado = 4,
    Concluido = 5
}

public enum PortePet
{
    Pequeno = 1,    // até 10kg
    Medio = 2,      // 10kg a 20kg
    Grande = 3,     // 20kg a 40kg
    GrandePorte = 4 // acima de 40kg
}