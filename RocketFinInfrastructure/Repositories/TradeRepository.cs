using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RocketFinDomain.Entities;
using RocketFinInfrastructure.Repositories.Interfaces;

namespace RocketFinInfrastructure.Repositories
{
    public class TradeRepository : Repository<Trade, PortfolioDbContext>, ITradeRepository
    {
        public TradeRepository(PortfolioDbContext context): base(context)
        {
        }

        public async Task AddTradeAsync(Trade trade)
        {
            _context.Trades.Add(trade);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetHoldingsAsync(string symbol)
        {
            var trades = await _context.Trades
                .Where(x => x.Symbol == symbol && (x.TradeType == TradeType.Buy || x.TradeType == TradeType.Sell))
                .ToListAsync();

            decimal totalHoldings = trades.Sum(t => t.TradeType == TradeType.Buy ? t.Quantity : -t.Quantity);

            return totalHoldings;
        }

        public async Task<IEnumerable<Trade>> GetTradesAsync(string symbol = null)
        {
            return await _context.Trades
                .Where(t => symbol == null || t.Symbol == symbol)
                .OrderBy(t => t.TransactionDate)
                .ToListAsync();
        }
    }
}
