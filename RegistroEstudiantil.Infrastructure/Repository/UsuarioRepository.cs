using Microsoft.EntityFrameworkCore;
using RegistroEstudiantil.Application.Interfaces.Persistence;
using RegistroEstudiantil.Domain.Entities;
using RegistroEstudiantil.Infrastructure.Data;

namespace RegistroEstudiantil.Infrastructure.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _db;

        public UsuarioRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<Usuario?> ObtenerPorCorreoAsync(string email) =>
            _db.Usuarios.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

        public Task<bool> ExisteCorreoAsync(string email, int? excludeId = null) =>
            _db.Usuarios.AnyAsync(x => x.Email == email && (excludeId == null || x.Id != excludeId));

        public void Agregar(Usuario usuario) => _db.Usuarios.Add(usuario);
    }
}
