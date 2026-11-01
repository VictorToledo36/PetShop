namespace PetShop.Domain.Entities;

public class MensagemAgendamento : EntidadeBase
{
    public int AgendamentoId { get; set; }
    public string ApplicationUserId { get; set; } = string.Empty;
    public bool EnviadaPeloAdmin { get; set; }
    public string Texto { get; set; } = string.Empty;
    public bool Lida { get; set; }
    public Agendamento Agendamento { get; set; } = null!;
}
