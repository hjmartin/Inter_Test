using RegistroEstudiantil.Domain.Entities;

namespace RegistroEstudiantil.Application.Interfaces.Persistence
{
    public interface IGrupoClaseRepository
    {
        Task<GrupoClase?> ObtenerPorIdAsync(int id);
    }
}
