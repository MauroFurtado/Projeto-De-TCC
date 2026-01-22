using ResrvaDeSala_API.Data;
using ResrvaDeSala_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using ResrvaDeSala_API.DTOs;
using Microsoft.EntityFrameworkCore; 
using System.Security.Cryptography;
using System.Text;

namespace ReservaDeSala_API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UsuariosController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //post/usuarios
    [HttpPost(Name = "CreateUsuario")]
    public async Task<ActionResult> CreateUsuarioAsync([FromBody] UsuarioCreateDto usuarioDto)
    {
        var usuario = _mapper.Map<UsuarioModel>(usuarioDto);
        usuario.Senha = HashPassword(usuarioDto.Senha);
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

        _mapper.Map(usuarioDto, usuario);
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

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}
