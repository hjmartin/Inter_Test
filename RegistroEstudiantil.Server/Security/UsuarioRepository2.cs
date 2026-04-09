using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RegistroEstudiantil.Server.Data;
using RegistroEstudiantil.Server.DTOs;
using RegistroEstudiantil.Server.DTOs.Auth;
using RegistroEstudiantil.Server.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RegistroEstudiantil.Server.Security.IReository
{
    public class UsuarioRepository2:IUsuarioRepository2
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public UsuarioRepository2(ApplicationDbContext db,
           IConfiguration config)
        {
            this._db = db;
            this._config = config;
        }

        //public async Task<LoginResponseDTO> AutenticarAsync(string correo, string contrasena)
        //{
        //    var usuario = await _db.Usuarios
        //        .FirstOrDefaultAsync(e => e.Correo_Electronico == correo);

        //    if (usuario == null || usuario.Pass_Usuario != contrasena)
        //        return null;

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name, usuario.Correo_Electronico),
        //        new Claim(ClaimTypes.NameIdentifier, usuario.Id_Persona_Usuario.ToString()),
        //        new Claim(ClaimTypes.Email, usuario.Correo_Electronico),
        //        new Claim(ClaimTypes.Role, "admin"),
        //        new Claim("Telefono", "3112345911"),

        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtKey"]!));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: _config["JwtIssuer"],
        //        audience: _config["JwtAudience"],
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddHours(1),
        //        signingCredentials: creds
        //    );

        //    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        //    return new LoginResponseDTO
        //    {
        //        Token = tokenString,
        //        Usuario_Id = usuario.Id_Persona_Usuario,
        //        Nombre = usuario.Correo_Electronico,
        //        Correo = usuario.Correo_Electronico,
        //        Expiracion = token.ValidTo
        //    };
        //}

      
        //public async Task<bool> CambiarContrasenaAsync(int usuarioId, string contrasenaActual, string nuevaContrasena)
        //{
        //    var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Id_Persona_Usuario == usuarioId);

        //    if (usuario == null || usuario.Pass_Usuario != contrasenaActual)
        //        return false;

        //    usuario.Pass_Usuario = nuevaContrasena;
        //    usuario.Pass_Encriptado = ""; // opcional
        //    usuario.Encriptado = false;

        //    _db.Usuarios.Update(usuario);
        //    await _db.SaveChangesAsync();

        //    return true;
        //}


        //public async Task<bool> RestablecerContrasenaAsync(string correo, string nuevaContrasena)
        //{
        //    var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Correo_Electronico == correo);

        //    if (usuario == null)
        //        return false;

        //    usuario.Pass_Usuario = nuevaContrasena;
        //    usuario.Pass_Encriptado = "";
        //    usuario.Encriptado = false;

        //    _db.Usuarios.Update(usuario);
        //    await _db.SaveChangesAsync();

        //    return true;
        //}


    }
}

