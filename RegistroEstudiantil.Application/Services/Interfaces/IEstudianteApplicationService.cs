using RegistroEstudiantil.Application.Common.Models;
using RegistroEstudiantil.Application.DTOs;
using RegistroEstudiantil.Application.DTOs.Shared;

namespace RegistroEstudiantil.Application.Services.Interfaces
{
    public interface IEstudianteApplicationService
    {
        Task<PagedResult<EstudianteDTO>> GetCurrentAsync(PaginacionDTO paginacionDTO);
        Task<IReadOnlyList<EstudianteDTO>> GetAllAsync();
        Task<EstudianteDTO> CreateAsync(EstudianteCreacionDTO dto);
        Task UpdateAsync(int id, EstudianteUpdateDTO dto);
        Task<EstudianteDTO> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}


