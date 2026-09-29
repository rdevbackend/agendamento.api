using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// Ajuste os 'using' abaixo conforme as pastas do seu projeto
using AgendamentoApi.Data;
using AgendamentoApi.Models;

namespace AgendamentoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AgendamentosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/agendamentos/barbeiro/Lucas Silva
        [HttpGet("barbeiro/{nomeBarbeiro}")]
        public async Task<IActionResult> GetPorBarbeiro(string nomeBarbeiro)
        {
            var agendamentosBarbeiro = await _context.Agendamentos
                .Where(a => a.Barbeiro.ToLower() == nomeBarbeiro.ToLower())
                .OrderBy(a => a.DataHora)
                .ToListAsync();

            return Ok(agendamentosBarbeiro);
        }

        // POST: api/agendamentos
        [HttpPost]
        public async Task<IActionResult> CriarAgendamento([FromBody] AgendamentoCriacaoDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dados de agendamento inválidos.");
            }

            var novoAgendamento = new Agendamento
            {
                ClienteNome = dto.NomeCliente,
                ClienteTelefone = dto.TelCliente,
                Barbeiro = dto.Barbeiro,
                DataHora = dto.DataHora,
                Servicos = string.Join(", ", dto.ServicosNomes),
                PrecoTotal = dto.PrecoTotal,
                Observacao = dto.Observacao ?? string.Empty,
                Status = "Confirmado"
            };

            _context.Agendamentos.Add(novoAgendamento);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Agendamento realizado com sucesso!", id = novoAgendamento.Id });
        }
    }

    // DTO utilizado para receber as requisições POST do frontend (index.html)
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