#nullable enable

using System.Data;

namespace SunemedicPRO_Inventarios.Server.Repositories.IRepository
{
    public interface IUnitOfWork
    {
        IUsuarioRepository UsuarioRepo { get; }
        IEstudianteRepository EstudianteRepo { get; }
        IInscripcionRepository InscripcionRepo { get; }

        void Dispose();
        Task SaveAsync();
        Task ExecuteInTransactionAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
