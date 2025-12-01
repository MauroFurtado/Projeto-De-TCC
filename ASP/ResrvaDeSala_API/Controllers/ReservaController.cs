using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Mvc;
using ResrvaDeSala_API.Data;
using ResrvaDeSala_API.Models;
using ResrvaDeSala_API.DTOs;
using AutoMapper;

namespace ReservaDeSala_API.Controllers;
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReservaController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //post: api/Reserva
        [HttpPost]
        public async Task<ActionResult<ReservaDto>> CreateReserva(ReservaDto ReservaDto)
        {
            var reserva = _mapper.Map<ReservaModel>(ReservaDto);
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            var createdReservaDto = _mapper.Map<ReservaDto>(reserva);
            return CreatedAtAction(nameof(GetReserva), new { id = reserva.Id }, createdReservaDto);
        }

        // GET: api/Reserva
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservaDto>>> GetReservas()
        {
            var reservas = await _context.Reservas.Include(r => r.Sala).ToListAsync();
            var reservasDTO = _mapper.Map<List<ReservaDto>>(reservas);
            return Ok(reservasDTO);
        }
        // GET: api/Reserva/id
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservaDto>> GetReserva(int id)
        {
            var reserva = await _context.Reservas.Include(r => r.Sala).FirstOrDefaultAsync(r => r.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }
            var ReservaDto = _mapper.Map<ReservaDto>(reserva);
            return Ok(ReservaDto);
        }

        //put: api/Reserva/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReserva(int id, ReservaDto ReservaDto)
        {
            if (id != ReservaDto.Id)
            {
                return BadRequest();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            _mapper.Map(ReservaDto, reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Reserva/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }