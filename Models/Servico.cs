namespace AgendamentoApi.Models;

public class Servico
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty; // Ex: Corte de Cabelo, Barba
    public decimal Preco { get; set; }
    public int DuracaoMinutos { get; set; } // Ex: 30
}