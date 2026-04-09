#nullable enable

using System.Data;

namespace RegistroEstudiantil.Application.Interfaces.Persistence
{
    public interface IUnitOfWork
    {
        IUsuarioRepository UsuarioRepo { get; }
        IEstudianteRepository EstudianteRepo { get; }
        IGrupoClaseRepository GrupoClaseRepo { get; }
        IInscripcionRepository InscripcionRepo { get; }

        void Dispose();
        Task GuardarCambiosAsync();
        Task EjecutarEnTransaccionAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
