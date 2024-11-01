using RocketFinDomain.Entities;

namespace RocketFinInfrastructure.Repositories.Interfaces
{
    public interface ITradeRepository: IRepository<Trade>
    {
        Task AddTradeAsync(Trade trade);
        Task<decimal> GetHoldingsAsync(string symbol);
        Task<IEnumerable<Trade>> GetTradesAsync(string symbol = null);
    }
}
