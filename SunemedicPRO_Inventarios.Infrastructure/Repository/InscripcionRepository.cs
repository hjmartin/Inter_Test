using Microsoft.EntityFrameworkCore;
using SunemedicPRO_Inventarios.Server.Data;
using SunemedicPRO_Inventarios.Server.DTOs;
using SunemedicPRO_Inventarios.Server.Entities;
using SunemedicPRO_Inventarios.Server.Repositories.IRepository;

namespace SunemedicPRO_Inventarios.Infrastructure.Repository
{
    public class InscripcionRepository : Repository<Inscripcion>, IInscripcionRepository
    {
        public InscripcionRepository(ApplicationDbContext db) : base(db) { }


        public async Task<IReadOnlyList<Inscripcion>> GetByEstudiantePeriodoAsync(int estudianteId, string periodo) =>
     await _db.Inscripciones
              .Include(i => i.GrupoClase).ThenInclude(g => g.Materia)
              .Include(i => i.GrupoClase).ThenInclude(g => g.Profesor)
              .Where(i => i.EstudianteId == estudianteId && i.Periodo == periodo)
              .AsNoTracking()
              .ToListAsync();

        public Task<int> CountByEstudiantePeriodoAsync(int estudianteId, string periodo) =>
            _db.Inscripciones.CountAsync(i => i.EstudianteId == estudianteId && i.Periodo == periodo);

        public Task<bool> ExistsProfesorEnPeriodoAsync(int estudianteId, int profesorId, string periodo) =>
            _db.Inscripciones.AnyAsync(i =>
                i.EstudianteId == estudianteId &&
                i.Periodo == periodo &&
                i.GrupoClase.ProfesorId == profesorId);

        public Task<bool> ExistsMateriaEnPeriodoAsync(int estudianteId, int materiaId, string periodo) =>
            _db.Inscripciones.AnyAsync(i =>
                i.EstudianteId == estudianteId &&
                i.Periodo == periodo &&
                i.GrupoClase.MateriaId == materiaId);

        public async Task<IReadOnlyList<string>> GetNombresCompanerosAsync(int grupoId, int excludeEstudianteId) =>
            await _db.Inscripciones
                     .Where(i => i.GrupoClaseId == grupoId && i.EstudianteId != excludeEstudianteId)
                     .Select(i => i.Estudiante.Nombres + " " + i.Estudiante.Apellidos)
                     .AsNoTracking()
                     .ToListAsync();

        public Task<int> CountInscritosGrupoAsync(int grupoId) =>
            _db.Inscripciones.CountAsync(i => i.GrupoClaseId == grupoId);

        public async Task<bool> HasCupoAsync(int grupoId)
        {
            var grupo = await _db.Grupos.AsNoTracking().FirstOrDefaultAsync(g => g.Id == grupoId);
            if (grupo is null) return false;
            var inscritos = await CountInscritosGrupoAsync(grupoId);
            return inscritos < grupo.Cupo;
        }

        public async Task<IReadOnlyList<GrupoInfoDto>> GetGruposInfoAsync()
        {
            var grupos = await _db.Grupos
                .Include(g => g.Materia)
                .Include(g => g.Profesor)
                .Select(g => new GrupoInfoDto
                {
                    Id = g.Id,
                    GrupoInfo = "Grupo: " + g.NombreGrupo +
                                ", Materia: " + g.Materia.Nombre +
                                ", Profesor: " + g.Profesor.Nombres + " " + g.Profesor.Apellidos
                })
                .AsNoTracking()
                .ToListAsync();

            return grupos;
        }

        public async Task<IReadOnlyList<InscripcionInfoDto>> GetInscripcionesByEstudianteAsync(int estudianteId)
        {
            var inscripciones = await _db.Inscripciones
                .Include(i => i.GrupoClase)
                    .ThenInclude(g => g.Materia)
                .Include(i => i.GrupoClase)
                    .ThenInclude(g => g.Profesor)
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

            return inscripciones;
        }

        // Método 1: Obtener todas las inscripciones de un estudiante
        public async Task<List<Inscripcion>> ObtenerPorEstudianteAsync(int estudianteId)
        {
            return  await _db.Inscripciones
                                 .Where(i => i.EstudianteId == estudianteId)
                                 .ToListAsync();
        }

        // Método 2: Obtener nombres de estudiantes que tengan inscripciones en ciertos grupos
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
