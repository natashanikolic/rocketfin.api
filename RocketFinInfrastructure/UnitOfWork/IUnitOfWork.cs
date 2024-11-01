using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketFinInfrastructure.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);

    }
}
