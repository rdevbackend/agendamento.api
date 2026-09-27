using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgendamentoApi.Data;
using AgendamentoApi.Models;

namespace AgendamentoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AgendamentoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Agendamento/servicos
        [HttpGet("servicos")]
        public async Task<ActionResult<IEnumerable<Servico>>> GetServicos()
        {
            return await _context.Servicos.ToListAsync();
        }

        // GET: api/Agendamento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agendamento>>> GetAgendamentos()
        {
            return await _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Servico)
                .ToListAsync();
        }

        // POST: api/Agendamento
        [HttpPost]
        public async Task<ActionResult<Agendamento>> CriarAgendamento(Agendamento agendamento)
        {
            var servico = await _context.Servicos.FindAsync(agendamento.ServicoId);
            if (servico == null)
            {
                return BadRequest("Serviço não encontrado.");
            }

            _context.Agendamentos.Add(agendamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAgendamentos), new { id = agendamento.Id }, agendamento);
        }

        // DELETE: api/Agendamento/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarAgendamento(int id)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento == null)
            {
                return NotFound("Agendamento não encontrado.");
            }

            _context.Agendamentos.Remove(agendamento);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna 204
        }
    }
}