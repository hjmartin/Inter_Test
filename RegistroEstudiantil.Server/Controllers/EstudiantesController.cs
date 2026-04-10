using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using RegistroEstudiantil.Application.DTOs;
using RegistroEstudiantil.Application.Services.Interfaces;


namespace RegistroEstudiantil.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "esestudiante")]
    public class EstudiantesController : ControllerBase
    {
        private readonly IEstudianteApplicationService _estudianteApplicationService;
        private readonly IOutputCacheStore _outputCacheStore;
        private const string CacheTag = "Estudiantes";

        public EstudiantesController(
            IEstudianteApplicationService estudianteApplicationService,
            IOutputCacheStore outputCacheStore)
        {
            _estudianteApplicationService = estudianteApplicationService;
            _outputCacheStore = outputCacheStore;
        }

        //[AllowAnonymous]
        [HttpGet("{id:int}", Name = "GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _estudianteApplicationService.ObtenerPorIdAsync(id));
        }

        [HttpPost]
        [OutputCache(Tags = [CacheTag])]
        public async Task<IActionResult> Crear([FromBody] EstudianteCreacionDTO dto)
        {
            var estudiante = await _estudianteApplicationService.CrearAsync(dto);
            await _outputCacheStore.EvictByTagAsync(CacheTag, default);
            return CreatedAtAction(nameof(GetById), new { id = estudiante.Id }, estudiante);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] EstudianteUpdateDTO dto)
        {
            await _estudianteApplicationService.ActualizarAsync(id, dto);
            await _outputCacheStore.EvictByTagAsync(CacheTag, default);
            return NoContent();
        }
        [HttpGet("todos")]
        [OutputCache(Tags = [CacheTag])]
        public async Task<List<EstudianteDTO>> GetTodos()
        {
            return (await _estudianteApplicationService.ObtenerTodosAsync()).ToList();
        }

        [HttpGet]
        [OutputCache(Tags = [CacheTag])]
        public async Task<List<EstudianteDTO>> Get()
        {
            return (await _estudianteApplicationService.ObtenerActualAsync()).ToList();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _estudianteApplicationService.EliminarAsync(id);
            await _outputCacheStore.EvictByTagAsync(CacheTag, default);
            return NoContent();
        }
    }
}

