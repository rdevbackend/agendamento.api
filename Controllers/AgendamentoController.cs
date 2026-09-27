using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgendamentoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentosController : ControllerBase
    {
        // Lista simulada de agendamentos em memória (substituir por base de dados se usares Entity Framework)
        private static readonly List<AgendamentoDto> Agendamentos = new()
        {
            new AgendamentoDto {
                Id = 1,
                Barbeiro = "Lucas Silva",
                ClienteNome = "João Silva",
                ClienteTelefone = "(16) 99888-7766",
                DataHora = DateTime.Today.AddHours(10),
                Servicos = "Corte de cabelo + Barba",
                TempoTotalMinutos = 60,
                PrecoTotal = 60.00m,
                Observacao = "Prefere degradê baixo"
            },
            new AgendamentoDto {
                Id = 2,
                Barbeiro = "Lucas Silva",
                ClienteNome = "Carlos Eduardo",
                ClienteTelefone = "(16) 99111-2233",
                DataHora = DateTime.Today.AddHours(14),
                Servicos = "Corte de cabelo",
                TempoTotalMinutos = 30,
                PrecoTotal = 35.00m,
                Observacao = ""
            },
            new AgendamentoDto {
                Id = 3,
                Barbeiro = "Kauan Borsan",
                ClienteNome = "Mateus Souza",
                ClienteTelefone = "(16) 98877-6655",
                DataHora = DateTime.Today.AddHours(11),
                Servicos = "Barba desenhada",
                TempoTotalMinutos = 30,
                PrecoTotal = 25.00m,
                Observacao = "Alergia a lâmina tradicional"
            }
        };

        // GET: api/agendamentos/barbeiro/Lucas Silva
        [HttpGet("barbeiro/{nomeBarbeiro}")]
        public IActionResult GetPorBarbeiro(string nomeBarbeiro)
        {
            var agendamentosBarbeiro = Agendamentos
                .Where(a => a.Barbeiro.Equals(nomeBarbeiro, StringComparison.OrdinalIgnoreCase))
                .OrderBy(a => a.DataHora)
                .ToList();

            return Ok(agendamentosBarbeiro);
        }
    }

    public class AgendamentoDto
    {
        public int Id { get; set; }
        public string Barbeiro { get; set; } = string.Empty;
        public string ClienteNome { get; set; } = string.Empty;
        public string ClienteTelefone { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
        public string Servicos { get; set; } = string.Empty;
        public int TempoTotalMinutos { get; set; }
        public decimal PrecoTotal { get; set; }
        public string Observacao { get; set; } = string.Empty;
    }
}