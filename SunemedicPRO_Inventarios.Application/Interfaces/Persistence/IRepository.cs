using System.Linq.Expressions;

namespace SunemedicPRO_Inventarios.Server.Repositories.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>>? predicate = null);
        void Add(T entity);
        void UpdateAsync(T entity);
        void Remove(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll(string incluirPropiedades = "");
        Task DeleteAsync(int id);
        Task<T> GetFirstOrDefaultAsync(
    Expression<Func<T, bool>> filtro = null,
    string incluirPropiedades = null);
    }
}
