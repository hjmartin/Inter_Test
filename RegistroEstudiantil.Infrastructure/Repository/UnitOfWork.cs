using Microsoft.EntityFrameworkCore;
using RegistroEstudiantil.Application.Common.Exceptions;
using RegistroEstudiantil.Application.Interfaces.Persistence;
using RegistroEstudiantil.Infrastructure.Data;
using System.Data;

namespace RegistroEstudiantil.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IUsuarioRepository UsuarioRepo { get; }
        public IEstudianteRepository EstudianteRepo { get; }
        public IGrupoClaseRepository GrupoClaseRepo { get; }
        public IInscripcionRepository InscripcionRepo { get; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            UsuarioRepo = new UsuarioRepository(_db);
            EstudianteRepo = new EstudianteRepository(_db);
            GrupoClaseRepo = new GrupoClaseRepository(_db);
            InscripcionRepo = new InscripcionRepository(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task GuardarCambiosAsync()
        {
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new ConflictException("No se pudo completar la operación por un conflicto de persistencia.", ex.Message);
            }
        }

        public async Task EjecutarEnTransaccionAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
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
                catch (DbUpdateException ex)
                {
                    await transaction.RollbackAsync();
                    throw new ConflictException("No se pudo completar la operación por un conflicto de persistencia.", ex.Message);
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
