namespace AgendamentoApi.Models
{
    public class AgendamentoCriacaoDto
    {
        public string NomeCliente { get; set; } = string.Empty;
        public string TelCliente { get; set; } = string.Empty;
        public string Barbeiro { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
        public List<string> ServicosNomes { get; set; } = new();
        public decimal PrecoTotal { get; set; }
        public string? Observacao { get; set; }
    }
}