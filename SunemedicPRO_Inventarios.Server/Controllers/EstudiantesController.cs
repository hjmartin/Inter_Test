using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SunemedicPRO_Inventarios.Server.Application.Services.Interfaces;
using SunemedicPRO_Inventarios.Server.DTOs;
using SunemedicPRO_Inventarios.Server.DTOs.Shared;

namespace SunemedicPRO_Inventarios.Server.Controllers
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

        [HttpGet]
        [OutputCache(Tags = [CacheTag])]
        public async Task<List<EstudianteDTO>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var result = await _estudianteApplicationService.GetCurrentAsync(paginacionDTO);
            HttpContext.Response.Headers.Append("cantidad-total-registros", result.TotalCount.ToString());
            return result.Items.ToList();
        }

        [HttpGet("todos")]
        [OutputCache(Tags = [CacheTag])]
        public async Task<List<EstudianteDTO>> GetTodos()
        {
            return (await _estudianteApplicationService.GetAllAsync()).ToList();
        }

        [HttpPost]
        [OutputCache(Tags = [CacheTag])]
        public async Task<IActionResult> Crear([FromBody] EstudianteCreacionDTO dto)
        {
            var estudiante = await _estudianteApplicationService.CreateAsync(dto);
            await _outputCacheStore.EvictByTagAsync(CacheTag, default);
            return CreatedAtAction(nameof(GetById), new { id = estudiante.Id }, estudiante);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] EstudianteUpdateDTO dto)
        {
            await _estudianteApplicationService.UpdateAsync(id, dto);
            await _outputCacheStore.EvictByTagAsync(CacheTag, default);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("{id:int}", Name = "GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _estudianteApplicationService.GetByIdAsync(id));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _estudianteApplicationService.DeleteAsync(id);
            await _outputCacheStore.EvictByTagAsync(CacheTag, default);
            return NoContent();
        }
    }
}
