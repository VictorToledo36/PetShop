namespace PetShop.Domain.Entities;

public class Servico : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public ICollection<ServicoPreco> Precos { get; set; } = new List<ServicoPreco>();
    public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}