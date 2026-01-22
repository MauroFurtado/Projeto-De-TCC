using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ResrvaDeSala_API.Services
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string email, string perfil);
    }

    public class JwtService : IJwtService
    {
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;

        public JwtService(IConfiguration configuration)
        {
            _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? configuration["Jwt:Secret"] ?? "seu-super-secreto-padrao-mudar-em-producao";
            _jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? configuration["Jwt:Issuer"] ?? "ReservaSalasAPI";
            _jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? configuration["Jwt:Audience"] ?? "ReservaSalasClients";
        }

        public string GenerateToken(int userId, string email, string perfil)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim("perfil", perfil)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}