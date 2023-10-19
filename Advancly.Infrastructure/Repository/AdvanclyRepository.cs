using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Advancly.Infrastructure.Repository
{
    public class AdvanclyRepository<T> : IAdvanclyRepository<T> where T : class,  new()
    {
        protected readonly AdvanclyDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public AdvanclyRepository(AdvanclyDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            _dbSet.Remove(await _dbSet.FindAsync(id));
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = _dbSet;
            if (includes != null)
            {
                foreach (var property in includes)
                {
                    query = query.Include(property);
                }
            }
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(expression);
        }
        public async Task UpdateAsync(int id, T entity)
        {
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
