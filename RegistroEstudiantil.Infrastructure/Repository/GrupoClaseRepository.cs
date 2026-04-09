using Microsoft.EntityFrameworkCore;
using RegistroEstudiantil.Application.Interfaces.Persistence;
using RegistroEstudiantil.Domain.Entities;
using RegistroEstudiantil.Infrastructure.Data;

namespace RegistroEstudiantil.Infrastructure.Repository
{
    public class GrupoClaseRepository : IGrupoClaseRepository
    {
        private readonly ApplicationDbContext _db;

        public GrupoClaseRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<GrupoClase?> ObtenerPorIdAsync(int id) =>
            _db.Grupos.FirstOrDefaultAsync(x => x.Id == id);
    }
}
