using Microsoft.EntityFrameworkCore;
using SunemedicPRO_Inventarios.Server.Data;
using SunemedicPRO_Inventarios.Server.Entities;
using SunemedicPRO_Inventarios.Server.Repositories.IRepository;

namespace SunemedicPRO_Inventarios.Infrastructure.Repository
{
    public class EstudianteRepository : Repository<Estudiante>, IEstudianteRepository
    {
        public EstudianteRepository(ApplicationDbContext db) : base(db) { }

       
    }
}
