using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // GET: api/agendamentos (Busca TODOS os agendamentos)
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var agendamentos = await _context.Agendamentos
                .OrderBy(a => a.DataHora)
                .ToListAsync();

            return Ok(agendamentos);
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
                return BadRequest(new { erro = "Dados de agendamento inválidos." });
            }

            try
            {
                var dataHoraUtc = DateTime.SpecifyKind(dto.DataHora, DateTimeKind.Utc);

                var novoAgendamento = new Agendamento
                {
                    ClienteNome = dto.NomeCliente ?? string.Empty,
                    ClienteTelefone = dto.TelCliente ?? string.Empty,
                    Barbeiro = dto.Barbeiro ?? string.Empty,
                    DataHora = dataHoraUtc,
                    Servicos = dto.ServicosNomes != null && dto.ServicosNomes.Any() 
                                ? string.Join(", ", dto.ServicosNomes) 
                                : "Nenhum serviço selecionado",
                    PrecoTotal = dto.PrecoTotal,
                    Observacao = dto.Observacao ?? string.Empty,
                    Status = "Confirmado"
                };

                _context.Agendamentos.Add(novoAgendamento);
                await _context.SaveChangesAsync();

                return Ok(new { mensagem = "Agendamento realizado com sucesso!", id = novoAgendamento.Id });
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[ERRO AO SALVAR AGENDAMENTO]: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[DETALHE DO BANCO]: {ex.InnerException.Message}");
                }
                Console.ResetColor();

                return StatusCode(500, new { 
                    erro = "Ocorreu um erro interno ao salvar na base de dados.",
                    detalhes = ex.InnerException != null ? ex.InnerException.Message : ex.Message 
                });
            }
        }

        // PATCH: api/agendamentos/5/status (Atualizar o status do agendamento)
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] StatusDto dto)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento == null)
            {
                return NotFound(new { erro = "Agendamento não encontrado." });
            }

            agendamento.Status = dto.Status;
            await _context.SaveChangesAsync();

            return Ok(agendamento);
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

    // DTO para atualizar status
    public class StatusDto
    {
        public string Status { get; set; } = string.Empty;
    }
}