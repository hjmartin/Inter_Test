using Microsoft.EntityFrameworkCore;
using SunemedicPRO_Inventarios.Server.Data;
using SunemedicPRO_Inventarios.Server.Entities;
using SunemedicPRO_Inventarios.Server.Repositories.IRepository;

namespace SunemedicPRO_Inventarios.Infrastructure.Repository
{
    public class EstudianteRepository : Repository<Estudiante>, IEstudianteRepository
    {
        public EstudianteRepository(ApplicationDbContext db) : base(db) { }

        // Buscar estudiante por documento
        public Task<Estudiante> GetByDocumentoAsync(string documento) =>
            _db.Estudiantes.AsNoTracking()
                           .FirstOrDefaultAsync(e => e.Documento == documento);

        // Obtener estudiante e incluir su Usuario
        public Task<Estudiante> GetByIdWithUsuarioAsync(int id) =>
            _db.Estudiantes
               .Include(e => e.Usuario)
               .AsNoTracking()
               .FirstOrDefaultAsync(e => e.Id == id);
    }
}
