using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RocketFinInfrastructure.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
    }
}
