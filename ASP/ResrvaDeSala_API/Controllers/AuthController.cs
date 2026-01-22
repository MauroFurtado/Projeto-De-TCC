using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResrvaDeSala_API.Data;
using ResrvaDeSala_API.Models;
using ResrvaDeSala_API.Services;
using System.Security.Cryptography;
using System.Text;

namespace ResrvaDeSala_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthController(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (usuario == null || !VerifyPassword(request.Senha, usuario.Senha))
            {
                return Unauthorized(new { error = "Credenciais inválidas" });
            }

            var token = _jwtService.GenerateToken(usuario.Id, usuario.Email, usuario.Perfil);
            return Ok(new { access = token, perfil = usuario.Perfil });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            var existe = await _context.Usuarios.AnyAsync(u => u.Email == request.Email);
            if (existe)
            {
                return BadRequest(new { error = "Email já cadastrado" });
            }

            var usuario = new UsuarioModel
            {
                Nome = request.Nome,
                Email = request.Email,
                Senha = HashPassword(request.Senha),
                Perfil = "comum"
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), new { id = usuario.Id }, usuario);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput.Equals(hash);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;
    }

    public class RegisterRequest
    {
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;
    }
}