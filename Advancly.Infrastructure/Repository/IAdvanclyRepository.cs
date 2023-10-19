using System.Linq.Expressions;

namespace Advancly.Infrastructure.Repository
{
    public interface IAdvanclyRepository<T> where T : class, new()
    {
        Task AddAsync(T entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T> GetAsync(Expression<Func<T, bool>> expression, List<string> includes = null);
        Task UpdateAsync(int id, T entity);
    }
}