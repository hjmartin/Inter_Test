using RegistroEstudiantil.Application.Common.Models;
using RegistroEstudiantil.Application.DTOs;
using RegistroEstudiantil.Application.DTOs.Shared;

namespace RegistroEstudiantil.Application.Services.Interfaces
{
    public interface IEstudianteApplicationService
    {
        Task<PagedResult<EstudianteDTO>> ObtenerActualAsync(PaginacionDTO paginacionDTO);
        Task<IReadOnlyList<EstudianteDTO>> ObtenerTodosAsync();
        Task<EstudianteDTO> CrearAsync(EstudianteCreacionDTO dto);
        Task ActualizarAsync(int id, EstudianteUpdateDTO dto);
        Task<EstudianteDTO> ObtenerPorIdAsync(int id);
        Task EliminarAsync(int id);
    }
}
