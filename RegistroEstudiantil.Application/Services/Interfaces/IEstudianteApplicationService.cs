using RegistroEstudiantil.Application.DTOs;

namespace RegistroEstudiantil.Application.Services.Interfaces
{
    public interface IEstudianteApplicationService
    {
        Task<IReadOnlyList<EstudianteDTO>> ObtenerActualAsync();
        Task<IReadOnlyList<EstudianteDTO>> ObtenerTodosAsync();
        Task<EstudianteDTO> CrearAsync(EstudianteCreacionDTO dto);
        Task ActualizarAsync(int id, EstudianteUpdateDTO dto);
        Task<EstudianteDTO> ObtenerPorIdAsync(int id);
        Task EliminarAsync(int id);
    }
}
