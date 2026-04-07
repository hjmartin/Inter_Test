using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunemedicPRO_Inventarios.Server.Application.Services.Interfaces;
using SunemedicPRO_Inventarios.Server.DTOs;

namespace SunemedicPRO_Inventarios.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "esestudiante")]
    public class InscripcionesController : ControllerBase
    {
        private readonly IInscripcionApplicationService _inscripcionApplicationService;

        public InscripcionesController(IInscripcionApplicationService inscripcionApplicationService)
        {
            _inscripcionApplicationService = inscripcionApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] InscripcionCreateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            await _inscripcionApplicationService.CrearAsync(dto);
            return Ok(new { mensaje = "Inscripción realizada." });
        }

        [HttpGet("{grupoId:int}/companeros")]
        public async Task<IActionResult> VerCompaneros(int grupoId)
        {
            return Ok(await _inscripcionApplicationService.VerCompanerosAsync(grupoId));
        }

        [HttpGet("de/{estudianteId:int}/{periodo}")]
        public async Task<IActionResult> VerRegistrosDeOtro(int estudianteId, string periodo)
        {
            return Ok(await _inscripcionApplicationService.VerRegistrosDeOtroAsync(estudianteId, periodo));
        }

        [HttpGet("mias/{periodo}/creditos")]
        public async Task<IActionResult> MisCreditos(string periodo)
        {
            return Ok(await _inscripcionApplicationService.MisCreditosAsync(periodo));
        }

        [HttpDelete("{inscripcionId:int}")]
        public async Task<IActionResult> Delete(int inscripcionId)
        {
            await _inscripcionApplicationService.DeleteAsync(inscripcionId);
            return NoContent();
        }

        [HttpGet("info")]
        public async Task<ActionResult<IEnumerable<GrupoInfoDto>>> GetGruposInfo()
        {
            return Ok(await _inscripcionApplicationService.GetGruposInfoAsync());
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InscripcionInfoDto>>> GetByEstudiante()
        {
            return Ok(await _inscripcionApplicationService.GetByEstudianteAsync());
        }

        [HttpGet("estudiante/{estudianteId:int}")]
        public async Task<ActionResult<IEnumerable<InscripcionInfoDto>>> GetByOtroEstudiante(int estudianteId)
        {
            return Ok(await _inscripcionApplicationService.GetByOtroEstudianteAsync(estudianteId));
        }

        [HttpGet("inscripciones-y-companieros")]
        public async Task<IActionResult> ObtenerInscripcionesYCompanieros()
        {
            return Ok(await _inscripcionApplicationService.ObtenerInscripcionesYCompanierosAsync());
        }
    }
}
