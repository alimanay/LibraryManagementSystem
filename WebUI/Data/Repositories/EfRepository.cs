using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Kütüphane_Yonetim_Sistemi.Context;

namespace Data.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        protected readonly LibraryContext _context;
        protected readonly DbSet<T> _dbSet;

        public EfRepository(LibraryContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<T?> FirstOrDefaultAsync(System.Linq.Expressions.Expression<System.Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
