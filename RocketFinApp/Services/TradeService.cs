using AutoMapper;
using Microsoft.Extensions.Logging;
using RocketFinApp.Models;
using RocketFinApp.Services.Interfaces;
using RocketFinDomain.Entities;
using RocketFinDomain.Requests;
using RocketFinDomain.Responses;
using RocketFinInfrastructure.Providers.Interfaces;
using RocketFinInfrastructure.Repositories.Interfaces;
using RocketFinInfrastructure.UnitOfWork;

namespace RocketFinApp.Services
{
    public class TradeService: ITradeService
    {
        private readonly ITradeRepository _tradeRepository;
        private readonly IMarketDataProvider _marketDataProvider;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TradeService> _logger;


        public TradeService(ITradeRepository tradeRepository, IMarketDataProvider marketDataProvider, IMapper mapper, IUnitOfWork unitOfWork, ILogger<TradeService> logger)
        {
            _tradeRepository = tradeRepository;
            _marketDataProvider = marketDataProvider;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<TransactionSummaryResponse>> GetRecentTransactionsAsync()
        {
            var trades = await _tradeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TransactionSummaryResponse>>(trades.OrderByDescending(x => x.TransactionDate).Take(5));
        }

        public async Task<IEnumerable<TransactionResponse>> GetTransactionsAsync(string symbol = null)
        {
            var trades = await _tradeRepository.GetAllAsync(t => symbol == null || t.Symbol == symbol);
            return _mapper.Map<IEnumerable<TransactionResponse>>(trades.OrderByDescending(x => x.TransactionDate));
        }

        public async Task BuyInstrumentAsync(BuyInstrumentRequest buyRequest)
        {
            _logger.LogInformation("Executing BuyInstrumentAsync ${symbol}", buyRequest.Symbol);

            var instrument = await _marketDataProvider.FetchInstrumentAsync(buyRequest.Symbol);

            var trade = _mapper.Map<Trade>(buyRequest);
            trade.PriceAtTransaction = instrument.CurrentPrice;

            await _tradeRepository.AddAsync(trade);
            await _unitOfWork.CompleteAsync();

        }

        public async Task SellInstrumentAsync(SellInstrumentRequest sellRequest)
        {
            _logger.LogInformation("Executing SellInstrumentAsync ${symbol}", sellRequest.Symbol);

            var instrument = await _marketDataProvider.FetchInstrumentAsync(sellRequest.Symbol);

            var currentHoldings = await _tradeRepository.GetHoldingsAsync(sellRequest.Symbol);

            if (currentHoldings < sellRequest.Quantity)
            {
                _logger.LogError("Failed to sell shares!");

                throw new InvalidOperationException("Insufficient shares to sell.");
            }

            var trade = _mapper.Map<Trade>(sellRequest);
            trade.PriceAtTransaction = instrument.CurrentPrice;

            await _tradeRepository.AddTradeAsync(trade);
        }
    }
}
