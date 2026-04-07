using Microsoft.EntityFrameworkCore;
using SunemedicPRO_Inventarios.Server.Data;
using SunemedicPRO_Inventarios.Server.Repositories.IRepository;
using System.Data;

namespace SunemedicPRO_Inventarios.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IUsuarioRepository UsuarioRepo { get; private set; }
        public IEstudianteRepository EstudianteRepo { get; private set; }
        public IInscripcionRepository InscripcionRepo { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            UsuarioRepo = new UsuarioRepository(_db);
            EstudianteRepo = new EstudianteRepository(_db);
            InscripcionRepo = new InscripcionRepository(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var strategy = _db.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _db.Database.BeginTransactionAsync(isolationLevel);

                try
                {
                    await action();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
    }
}
