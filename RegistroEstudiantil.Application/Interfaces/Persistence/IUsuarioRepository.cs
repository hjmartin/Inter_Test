using RegistroEstudiantil.Domain.Entities;

namespace RegistroEstudiantil.Application.Interfaces.Persistence
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObtenerPorCorreoAsync(string email);
        Task<bool> ExisteCorreoAsync(string email, int? excludeId = null);
        void Agregar(Usuario usuario);
    }
}
