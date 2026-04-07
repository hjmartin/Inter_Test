using Microsoft.EntityFrameworkCore;
using SunemedicPRO_Inventarios.Server.Data;
using SunemedicPRO_Inventarios.Server.Repositories.IRepository;
using System.Linq.Expressions;

namespace SunemedicPRO_Inventarios.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _db;
        protected readonly DbSet<T> _set;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _set = db.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id) => await _set.FindAsync(id);

        public virtual async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>>? predicate = null)
            => predicate is null
               ? await _set.AsNoTracking().ToListAsync()
               : await _set.AsNoTracking().Where(predicate).ToListAsync();

        public void Add(T entity)
        {
            _set.Add(entity);
        }

        public void UpdateAsync(T entity)
        {
            _set.Update(entity);
        }

        public void Remove(T entity)
        {
            _set.Remove(entity);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
            => _set.AnyAsync(predicate);

        public IQueryable<T> GetAll(string incluirPropiedades = "")
        {
            IQueryable<T> query = _set;

            if (!string.IsNullOrWhiteSpace(incluirPropiedades))
            {
                foreach (var propiedad in incluirPropiedades.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propiedad.Trim());
                }
            }

            return query;
        }

        public async Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>>? filtro = null,
            string? incluirPropiedades = null)
        {
            IQueryable<T> query = _set;

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            if (incluirPropiedades != null)
            {
                foreach (var item in incluirPropiedades.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _set.FindAsync(id);

            if (entity is not null)
            {
                _set.Remove(entity);
            }
        }
    }
}
