using AutoMapper;
using Microsoft.Extensions.Logging;
using RocketFinApp.Models;
using RocketFinApp.Services.Interfaces;
using RocketFinDomain.Entities;
using RocketFinDomain.Responses;
using RocketFinInfrastructure.Providers.Interfaces;
using RocketFinInfrastructure.Repositories.Interfaces;

namespace RocketFinApp.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly ITradeRepository _tradeRepository;
        private readonly IMarketDataProvider _marketDataProvider;
        private readonly IMapper _mapper;
        private readonly ILogger<PortfolioService> _logger;

        public PortfolioService(ITradeRepository tradeRepository, IMarketDataProvider marketDataProvider, IMapper mapper, ILogger<PortfolioService> logger)
        {
            _tradeRepository = tradeRepository;
            _marketDataProvider = marketDataProvider;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<PortfolioResponse>> GetPortfolioAsync(string symbol = null)
        {
            _logger.LogInformation("Executing GetPortfolioAsync ${symbol}", symbol);

            var trades = await _tradeRepository.GetAllAsync(t => symbol == null || t.Symbol == symbol);

            var holdings = trades
                .GroupBy(t => t.Symbol)
                .Select(g => new Holding
                {
                    Symbol = g.Key,
                    Quantity = g.Sum(t => t.Quantity),
                    TotalShares = g.Sum(t => t.TradeType == TradeType.Buy ? t.Quantity : -t.Quantity),
                    CostBasis = g.Where(t => t.TradeType == TradeType.Buy).Sum(t => t.Quantity * t.PriceAtTransaction)
                });

            var portfolioResponses = new List<PortfolioResponse>();

            foreach (var holding in holdings)
            {
                _logger.LogInformation("Executing FetchInstrumentAsync ${symbol}", holding.Symbol);

                var instrument = await _marketDataProvider.FetchInstrumentAsync(holding.Symbol);
                portfolioResponses.Add(_mapper.Map<PortfolioResponse>((holding, instrument)));
            }

            return portfolioResponses;
        }
    }
}
