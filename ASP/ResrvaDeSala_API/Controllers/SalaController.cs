using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Mvc;
using ResrvaDeSala_API.Data;
using ResrvaDeSala_API.Models;
using ResrvaDeSala_API.DTOs;
using AutoMapper;

namespace ReservaDeSala_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SalaController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Sala
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalaDto>>> GetSalas()
        {
            var salas = await _context.Salas.ToListAsync();
            var salasDto = _mapper.Map<List<SalaDto>>(salas);
            return Ok(salasDto);
        }

        // GET: api/Sala/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SalaDto>> GetSala(int id)
        {
            var sala = await _context.Salas.FindAsync(id);
            if (sala == null) return NotFound();

            var salaDto = _mapper.Map<SalaDto>(sala);
            return Ok(salaDto);
        }

        // POST: api/Sala
        [HttpPost]
        public async Task<ActionResult<SalaDto>> CreateSala(SalaDto salaDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var sala = _mapper.Map<SalaModel>(salaDto);
            _context.Salas.Add(sala);
            await _context.SaveChangesAsync();

            var createdSalaDto = _mapper.Map<SalaDto>(sala);
            return CreatedAtAction(nameof(GetSala), new { id = sala.Id }, createdSalaDto);
        }

        // PUT: api/Sala/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSala(int id, SalaDto salaDto)
        {
            if (id != salaDto.Id) return BadRequest();

            var sala = await _context.Salas.FindAsync(id);
            if (sala == null) return NotFound();

            _mapper.Map(salaDto, sala);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Sala/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSala(int id)
        {
            var sala = await _context.Salas.FindAsync(id);
            if (sala == null) return NotFound();

            _context.Salas.Remove(sala);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
