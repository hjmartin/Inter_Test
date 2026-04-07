using Microsoft.EntityFrameworkCore;
using SunemedicPRO_Inventarios.Server.Data;
using SunemedicPRO_Inventarios.Server.Entities;
using SunemedicPRO_Inventarios.Server.Repositories.IRepository;

namespace SunemedicPRO_Inventarios.Infrastructure.Repository
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
