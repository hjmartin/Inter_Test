using Microsoft.EntityFrameworkCore;
using RegistroEstudiantil.Application.Interfaces.Persistence;
using RegistroEstudiantil.Domain.Entities;
using RegistroEstudiantil.Infrastructure.Data;

namespace RegistroEstudiantil.Infrastructure.Repository
{
    public class EstudianteRepository : IEstudianteRepository
    {
        private readonly ApplicationDbContext _db;

        public EstudianteRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<Estudiante?> ObtenerPorIdAsync(int id) =>
            _db.Estudiantes.FirstOrDefaultAsync(x => x.Id == id);

        public Task<Estudiante?> ObtenerPorUsuarioIdAsync(int usuarioId) =>
            _db.Estudiantes.AsNoTracking().FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);

        public async Task<IReadOnlyList<Estudiante>> ObtenerTodosOrdenadosAsync() =>
            await _db.Estudiantes
                .AsNoTracking()
                .OrderBy(x => x.Nombres)
                .ToListAsync();

        public async Task<IReadOnlyList<Estudiante>> ObtenerPaginaPorUsuarioAsync(int usuarioId, int page, int pageSize) =>
            await _db.Estudiantes
                .AsNoTracking()
                .Where(x => x.UsuarioId == usuarioId)
                .OrderBy(x => x.Nombres)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public Task<int> ContarPorUsuarioAsync(int usuarioId) =>
            _db.Estudiantes.CountAsync(x => x.UsuarioId == usuarioId);

        public Task<bool> ExistePorUsuarioIdAsync(int usuarioId) =>
            _db.Estudiantes.AnyAsync(x => x.UsuarioId == usuarioId);

        public Task<bool> ExistePorDocumentoAsync(string documento) =>
            _db.Estudiantes.AnyAsync(x => x.Documento == documento);

        public void Agregar(Estudiante estudiante) => _db.Estudiantes.Add(estudiante);

        public void Actualizar(Estudiante estudiante) => _db.Estudiantes.Update(estudiante);

        public void Eliminar(Estudiante estudiante) => _db.Estudiantes.Remove(estudiante);
    }
}
