namespace AgendamentoApi.Models
{
    public class Agendamento
    {
        public int Id { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public string ClienteTelefone { get; set; } = string.Empty;
        public string Barbeiro { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
        public string Servicos { get; set; } = string.Empty;
        public decimal PrecoTotal { get; set; }
        public string Status { get; set; } = "Confirmado";
        public string? Observacao { get; set; }
    }
}