using SunemedicPRO_Inventarios.Server.Application.Common.Models;
using SunemedicPRO_Inventarios.Server.DTOs;
using SunemedicPRO_Inventarios.Server.DTOs.Shared;

namespace SunemedicPRO_Inventarios.Server.Application.Services.Interfaces
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
