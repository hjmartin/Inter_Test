using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RegistroEstudiantil.Application.DTOs.Auth;
using RegistroEstudiantil.Domain.Entities;
using RegistroEstudiantil.Application.Interfaces.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RegistroEstudiantil.Infrastructure.Security
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _cfg;

        public JwtTokenService(IConfiguration configuration)
        {
            _cfg = configuration;
        }

        public LoginResponseDTO CreateToken(Usuario u)
        {
            var issuer = _cfg["JwtIssuer"]!;
            var audience = _cfg["JwtAudience"]!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["JwtKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, u.Id.ToString()),
                new(ClaimTypes.Email, u.Email),
                new(ClaimTypes.Role, u.Rol ?? "Estudiante"),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var minutesStr = _cfg["JwtExpireMinutes"];
            var expires = DateTime.UtcNow.AddMinutes(int.TryParse(minutesStr, out var m) ? m : 120);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponseDTO
            {
                Token = tokenString,
                Usuario_Id = u.Id,
                Nombre = u.Email,
                Correo = u.Email,
                Expiracion = token.ValidTo
            };
        }
    }
}


