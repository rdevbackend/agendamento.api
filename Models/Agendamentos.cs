namespace AgendamentoApi.Models;

public class Agendamento
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public int ServicoId { get; set; }
    public Servico? Servico { get; set; }
    public DateTime DataHora { get; set; }
    public string Status { get; set; } = "Confirmado"; // Confirmado, Cancelado, Concluido
}