using PetShop.Domain.Enums;

namespace PetShop.Domain.Entities;

public class Agendamento : EntidadeBase
{
    public string ApplicationUserId { get; set; } = string.Empty;
    public int PetId { get; set; }
    public int ServicoId { get; set; }
    public DateTime DataSolicitada { get; set; }
    public TimeOnly HorarioSolicitado { get; set; }
    public StatusAgendamento Status { get; set; } = StatusAgendamento.Pendente;
    public string? ObservacaoCliente { get; set; }  // cliente descreve algo sobre o pet
    public string? MotivoRecusa { get; set; }        // admin preenche se recusar
    public DateTime? DataResposta { get; set; }      // quando admin respondeu

    // Navegação
    public Pet Pet { get; set; } = null!;
    public Servico Servico { get; set; } = null!;
}