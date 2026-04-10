using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistroEstudiantil.Application.DTOs.Auth;
using RegistroEstudiantil.Application.Services.Interfaces;
namespace RegistroEstudiantil.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "esestudiante")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioApplicationService _usuarioApplicationService;

        public UsuariosController(IUsuarioApplicationService usuarioApplicationService)
        {
            _usuarioApplicationService = usuarioApplicationService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var respuesta = await _usuarioApplicationService.RegistrarAsync(dto);
            return Ok(respuesta);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto)
        {
            var respuesta = await _usuarioApplicationService.IniciarSesionAsync(dto);
            return Ok(respuesta);
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(_usuarioApplicationService.ObtenerUsuarioActual());
        }
    }
}

