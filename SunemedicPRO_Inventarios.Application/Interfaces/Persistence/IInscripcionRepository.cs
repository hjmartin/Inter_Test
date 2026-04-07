using SunemedicPRO_Inventarios.Server.DTOs;
using SunemedicPRO_Inventarios.Server.Entities;

namespace SunemedicPRO_Inventarios.Server.Repositories.IRepository
{
    public interface IInscripcionRepository : IRepository<Inscripcion>
    {
        Task<IReadOnlyList<Inscripcion>> GetByEstudiantePeriodoAsync(int estudianteId, string periodo);
        Task<int> CountByEstudiantePeriodoAsync(int estudianteId, string periodo);
        Task<bool> ExistsProfesorEnPeriodoAsync(int estudianteId, int profesorId, string periodo);
        Task<bool> ExistsMateriaEnPeriodoAsync(int estudianteId, int materiaId, string periodo);
        Task<IReadOnlyList<string>> GetNombresCompanerosAsync(int grupoId, int excludeEstudianteId);
        Task<int> CountInscritosGrupoAsync(int grupoId);
        Task<bool> HasCupoAsync(int grupoId);
        Task<IReadOnlyList<GrupoInfoDto>> GetGruposInfoAsync();
        Task<IReadOnlyList<InscripcionInfoDto>> GetInscripcionesByEstudianteAsync(int estudianteId);

        // Método 1: Obtener todas las inscripciones de un estudiante
        Task<List<Inscripcion>> ObtenerPorEstudianteAsync(int estudianteId);


        Task<List<string>> ObtenerNombresPorGruposAsync(List<int> grupoIds);
    }
}
