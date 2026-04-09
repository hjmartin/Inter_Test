#nullable enable

using System.Data;

namespace RegistroEstudiantil.Application.Interfaces.Persistence
{
    public interface IUnitOfWork
    {
        IUsuarioRepository UsuarioRepo { get; }
        IInscripcionRepository InscripcionRepo { get; }
        IRepository<T> Repository<T>() where T : class;

        void Dispose();
        Task SaveAsync();
        Task ExecuteInTransactionAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}


