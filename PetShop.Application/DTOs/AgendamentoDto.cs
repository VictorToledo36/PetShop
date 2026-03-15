using PetShop.Domain.Enums;

namespace PetShop.Application.DTOs;

public class AgendamentoDto
{
    public int Id { get; set; }
    public string PetNome { get; set; } = string.Empty;
    public string ServicoNome { get; set; } = string.Empty;
    public DateTime DataSolicitada { get; set; }
    public TimeOnly HorarioSolicitado { get; set; }
    public StatusAgendamento Status { get; set; }
    public string? ObservacaoCliente { get; set; }
    public string? MotivoRecusa { get; set; }
}
public class SolicitarAgendamentoDto
{
    public int PetId { get; set; }
    public int ServicoId { get; set; }
    public DateTime DataSolicitada { get; set; }
    public TimeOnly HorarioSolicitado { get; set; }
    public string? ObservacaoCliente { get; set; }
}