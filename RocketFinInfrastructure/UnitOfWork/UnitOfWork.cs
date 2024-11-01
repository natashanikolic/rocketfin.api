using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RocketFinInfrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PortfolioDbContext _dbContext;

        public UnitOfWork(PortfolioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
