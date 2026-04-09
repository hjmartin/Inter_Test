using Microsoft.EntityFrameworkCore;
using RegistroEstudiantil.Infrastructure.Data;
using RegistroEstudiantil.Domain.Entities;
using RegistroEstudiantil.Application.Interfaces.Persistence;

namespace RegistroEstudiantil.Infrastructure.Repository
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext db) : base(db) { }

        public Task<Usuario?> GetByEmailAsync(string email) =>
            _db.Usuarios.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

        public Task<bool> ExistsEmailAsync(string email, int? excludeId = null) =>
            _db.Usuarios.AnyAsync(x => x.Email == email && (excludeId == null || x.Id != excludeId));
    }
}


