using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RocketFinInfrastructure.Repositories.Interfaces;

namespace RocketFinInfrastructure.Repositories
{
    public class Repository<T, C> : IRepository<T> where T : class where C : DbContext
    {
        protected readonly C _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(C context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }
    }
}
