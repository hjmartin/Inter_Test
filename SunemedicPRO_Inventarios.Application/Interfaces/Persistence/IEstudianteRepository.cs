using SunemedicPRO_Inventarios.Server.Entities;

namespace SunemedicPRO_Inventarios.Server.Repositories.IRepository
{
    public interface IEstudianteRepository : IRepository<Estudiante>
    {
        Task<Estudiante> GetByDocumentoAsync(string documento);
        Task<Estudiante> GetByIdWithUsuarioAsync(int id);
    }
}
