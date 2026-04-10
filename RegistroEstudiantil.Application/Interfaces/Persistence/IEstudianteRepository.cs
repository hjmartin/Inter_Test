using RegistroEstudiantil.Domain.Entities;

namespace RegistroEstudiantil.Application.Interfaces.Persistence
{
    public interface IEstudianteRepository
    {
        Task<Estudiante?> ObtenerPorIdAsync(int id);
        Task<Estudiante?> ObtenerPorUsuarioIdAsync(int usuarioId);
        Task<IReadOnlyList<Estudiante>> ObtenerTodosOrdenadosAsync();
        Task<IReadOnlyList<Estudiante>> ObtenerPorUsuarioAsync(int usuarioId);
        Task<bool> ExistePorUsuarioIdAsync(int usuarioId);
        Task<bool> ExistePorDocumentoAsync(string documento);
        void Agregar(Estudiante estudiante);
        void Actualizar(Estudiante estudiante);
        void Eliminar(Estudiante estudiante);
    }
}
