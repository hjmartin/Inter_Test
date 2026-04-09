using RegistroEstudiantil.Application.DTOs;
using RegistroEstudiantil.Domain.Entities;

namespace RegistroEstudiantil.Application.Interfaces.Persistence
{
    public interface IInscripcionRepository
    {
        Task<Inscripcion?> ObtenerPorIdAsync(int id);
        void Agregar(Inscripcion inscripcion);
        Task EliminarAsync(int id);
        Task<bool> ExisteAlgunaPorEstudianteAsync(int estudianteId);
        Task<IReadOnlyList<Inscripcion>> ObtenerPorEstudiantePeriodoAsync(int estudianteId, string periodo);
        Task<int> ContarPorEstudiantePeriodoAsync(int estudianteId, string periodo);
        Task<bool> ExisteProfesorEnPeriodoAsync(int estudianteId, int profesorId, string periodo);
        Task<bool> ExisteMateriaEnPeriodoAsync(int estudianteId, int materiaId, string periodo);
        Task<IReadOnlyList<string>> ObtenerNombresCompanerosAsync(int grupoId, int excludeEstudianteId);
     
        Task<IReadOnlyList<GrupoInfoDto>> ObtenerInfoGruposAsync();
        Task<IReadOnlyList<InscripcionInfoDto>> ObtenerInscripcionesPorEstudianteAsync(int estudianteId);
        Task<List<Inscripcion>> ObtenerPorEstudianteAsync(int estudianteId);
        Task<List<string>> ObtenerNombresPorGruposAsync(List<int> grupoIds);
    }
}
