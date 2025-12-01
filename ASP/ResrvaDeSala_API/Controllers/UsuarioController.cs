using ResrvaDeSala_API.Data;
using ResrvaDeSala_API.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ResrvaDeSala_API.DTOs;
using Microsoft.EntityFrameworkCore; 

namespace ReservaDeSala_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UsuarioController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //post/usuarios
    [HttpPost(Name = "CreateUsuario")]
    public async Task<ActionResult> CreateUsuarioAsync([FromBody] UsuarioCreateDto usuarioDto)
    {
        var usuario = _mapper.Map<UsuarioModel>(usuarioDto);
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var usuarioReadDto = _mapper.Map<UsuarioDto>(usuario);

        return CreatedAtRoute("GetUsuarioById", new { id = usuario.Id }, usuarioReadDto);
    }

    //get/usuarios
    [HttpGet(Name = "GetUsuarios")]
    public async Task<ActionResult<List<UsuarioDto>>> GetUsuariosAsync()
    {
        var usuarios = await _context.Usuarios.AsNoTracking().ToListAsync();
        var dtos = _mapper.Map<List<UsuarioDto>>(usuarios);
        return Ok(dtos);
    }

    //get/usuarios/{id}
    [HttpGet("{id:int}", Name = "GetUsuarioById")]
    public async Task<ActionResult<UsuarioDto>> GetUsuarioByIdAsync(int id)
    {
        var usuario = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        if (usuario == null)
        {
            return NotFound();
        }

        var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
        return Ok(usuarioDto);
    }

    //put/usuarios/{id}
    [HttpPut("{id:int}", Name = "UpdateUsuario")]
    public async Task<ActionResult> UpdateUsuarioAsync(int id, [FromBody] UsuarioDto usuarioDto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        if (usuario == null)
        {
            return NotFound();
        }

        _mapper.Map(usuarioDto, usuario); // requires mapping UsuarioDto -> UsuarioModel
        await _context.SaveChangesAsync();

        return NoContent();
    }

    //delete/usuarios/{id}
    [HttpDelete("{id:int}", Name = "DeleteUsuario")]
    public async Task<ActionResult> DeleteUsuarioAsync(int id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        if (usuario == null)
        {
            return NotFound();
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
