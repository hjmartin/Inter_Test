using Microsoft.EntityFrameworkCore;
using RegistroEstudiantil.Infrastructure.Data;
using RegistroEstudiantil.Application.Interfaces.Persistence;
using System.Data;

namespace RegistroEstudiantil.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private readonly Dictionary<Type, object> _repositories = new();

        public IUsuarioRepository UsuarioRepo { get; private set; }
        public IInscripcionRepository InscripcionRepo { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            UsuarioRepo = new UsuarioRepository(_db);
            InscripcionRepo = new InscripcionRepository(_db);
        }

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (!_repositories.TryGetValue(type, out var repository))
            {
                repository = new Repository<T>(_db);
                _repositories[type] = repository;
            }

            return (IRepository<T>)repository;
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


