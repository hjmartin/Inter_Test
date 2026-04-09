using RegistroEstudiantil.Domain.Entities;

namespace RegistroEstudiantil.Application.Interfaces.Persistence
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<bool> ExistsEmailAsync(string email, int? excludeId = null);
    }
}


