using Microsoft.EntityFrameworkCore;
using RegistroEstudiantil.Application.DTOs;
using RegistroEstudiantil.Application.Interfaces.Persistence;
using RegistroEstudiantil.Domain.Entities;
using RegistroEstudiantil.Infrastructure.Data;

namespace RegistroEstudiantil.Infrastructure.Repository
{
    public class InscripcionRepository : IInscripcionRepository
    {
        private readonly ApplicationDbContext _db;

        public InscripcionRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        //si
        public Task<Inscripcion?> ObtenerPorIdAsync(int id) =>
            _db.Inscripciones.FirstOrDefaultAsync(x => x.Id == id);

        public void Agregar(Inscripcion inscripcion) => _db.Inscripciones.Add(inscripcion);
        //si
        public async Task EliminarAsync(int id)
        {
            var entity = await _db.Inscripciones.FindAsync(id);
            if (entity is not null)
            {
                _db.Inscripciones.Remove(entity);
            }
        }

        public Task<bool> ExisteAlgunaPorEstudianteAsync(int estudianteId) =>
            _db.Inscripciones.AnyAsync(i => i.EstudianteId == estudianteId);
        //si
        public async Task<IReadOnlyList<Inscripcion>> ObtenerPorEstudiantePeriodoAsync(int estudianteId, string periodo) =>
            await _db.Inscripciones
                .Include(i => i.GrupoClase).ThenInclude(g => g.Materia)
                .Include(i => i.GrupoClase).ThenInclude(g => g.Profesor)
                .Where(i => i.EstudianteId == estudianteId && i.Periodo == periodo)
                .AsNoTracking()
                .ToListAsync();
        //si
        public Task<int> ContarPorEstudiantePeriodoAsync(int estudianteId, string periodo) =>
           _db.Inscripciones.CountAsync(i => i.EstudianteId == estudianteId && i.Periodo == periodo);
        //si
        public Task<bool> ExisteProfesorEnPeriodoAsync(int estudianteId, int profesorId, string periodo) =>
            _db.Inscripciones.AnyAsync(i =>
                i.EstudianteId == estudianteId &&
                i.Periodo == periodo &&
                i.GrupoClase.ProfesorId == profesorId);
        //pendiente
        public Task<bool> ExisteMateriaEnPeriodoAsync(int estudianteId, int materiaId, string periodo) =>
            _db.Inscripciones.AnyAsync(i =>
                i.EstudianteId == estudianteId &&
                i.Periodo == periodo &&
                i.GrupoClase.MateriaId == materiaId);
        //si
        public async Task<IReadOnlyList<string>> ObtenerNombresCompanerosAsync(int grupoId, int excludeEstudianteId) =>
            await _db.Inscripciones
                .Where(i => i.GrupoClaseId == grupoId && i.EstudianteId != excludeEstudianteId)
                .Select(i => i.Estudiante.Nombres + " " + i.Estudiante.Apellidos)
                .AsNoTracking()
                .ToListAsync();

      
        //si
        public async Task<IReadOnlyList<GrupoInfoDto>> ObtenerInfoGruposAsync()
        {
            return await _db.Grupos
                .Include(g => g.Materia)
                .Include(g => g.Profesor)
                .Select(g => new GrupoInfoDto
                {
                    Id = g.Id,
                    GrupoInfo = "Materia: " + g.Materia.Nombre +
                                ", Profesor: " + g.Profesor.Nombres + " " + g.Profesor.Apellidos
                })
                .AsNoTracking()
                .ToListAsync();
        }
        //si
        public async Task<IReadOnlyList<InscripcionInfoDto>> ObtenerInscripcionesPorEstudianteAsync(int estudianteId)
        {
            return await _db.Inscripciones
                .Include(i => i.GrupoClase).ThenInclude(g => g.Materia)
                .Include(i => i.GrupoClase).ThenInclude(g => g.Profesor)
                .Where(i => i.EstudianteId == estudianteId)
                .Select(i => new InscripcionInfoDto
                {
                    Id = i.Id,
                    NombreGrupo = i.GrupoClase.NombreGrupo,
                    Materia = i.GrupoClase.Materia.Nombre,
                    Profesor = i.GrupoClase.Profesor.Nombres + " " + i.GrupoClase.Profesor.Apellidos,
                    Creditos = i.GrupoClase.Materia.Creditos
                })
                .AsNoTracking()
                .ToListAsync();
        }
        //si
        public async Task<List<Inscripcion>> ObtenerPorEstudianteAsync(int estudianteId)
        {
            return await _db.Inscripciones
                .Where(i => i.EstudianteId == estudianteId)
                .ToListAsync();
        }
        //si
        public async Task<List<string>> ObtenerNombresPorGruposAsync(List<int> grupoIds)
        {
            return await _db.Inscripciones
                .Where(i => grupoIds.Contains(i.GrupoClaseId))
                .Include(i => i.Estudiante)
                .Select(i => i.Estudiante.Nombres)
                .Distinct()
                .ToListAsync();
        }
    }
}
