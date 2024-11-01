using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using RocketFinApp.Services;
using RocketFinDomain;
using RocketFinDomain.Entities;
using RocketFinDomain.Requests;
using RocketFinDomain.Responses;
using RocketFinInfrastructure.Providers.Interfaces;
using RocketFinInfrastructure.Repositories.Interfaces;
using RocketFinInfrastructure.UnitOfWork;

namespace UnitTests
{
    public class TradeServiceTests
    {
        private readonly Mock<ITradeRepository> _tradeRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IMarketDataProvider> _marketDataProviderMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<TradeService>> _loggerMock;
        private readonly TradeService _service;

        public TradeServiceTests()
        {
            _tradeRepositoryMock = new Mock<ITradeRepository>();
            _mapperMock = new Mock<IMapper>();
            _marketDataProviderMock = new Mock<IMarketDataProvider>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<TradeService>>();

            _service = new TradeService(
                _tradeRepositoryMock.Object,
                _marketDataProviderMock.Object,
                _mapperMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetRecentTransactionsAsync_ReturnsMostRecentFiveTransactions()
        {
            // Arrange
            var trades = new List<Trade>
            {
                new Trade { Symbol = "AAPL", Quantity= 2, TradeType = TradeType.Buy, TransactionDate = DateTime.Now.AddDays(-1) },
                new Trade { Symbol = "TSLA", Quantity = 5, TradeType = TradeType.Buy, TransactionDate = DateTime.Now.AddDays(-2) },
            };

            var transactionSummaries = trades.Select(trade => new TransactionSummaryResponse
            {
                Transaction = trade.Symbol,
                Amount = trade.Quantity * 100m,
                Date = trade.TransactionDate,
                Operation = trade.TradeType.ToString()
            }).ToList();

            _tradeRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Trade, bool>>>())).ReturnsAsync(trades);
            _mapperMock.Setup(m => m.Map<IEnumerable<TransactionSummaryResponse>>(It.IsAny<IEnumerable<Trade>>()))
                       .Returns(transactionSummaries);

            // Act
            var result = await _service.GetRecentTransactionsAsync();

            // Assert
            _tradeRepositoryMock.Verify(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Trade, bool>>>()), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<TransactionSummaryResponse>>(It.IsAny<IEnumerable<Trade>>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Theory]
        [InlineData("AAPL")]
        [InlineData(null)]
        public async Task GetTransactionsAsync_ReturnsTransactionsFilteredBySymbol(string symbol)
        {
            // Arrange
            var trades = new List<Trade> { new Trade { Symbol = "AAPL", TransactionDate = DateTime.Now } };

            var transactionSummaries = trades.Select(trade => new TransactionResponse
            {
                Symbol = trade.Symbol,
                Operation = trade.TradeType.ToString(),
                Quantity = trade.Quantity,
                PriceAtTransaction = trade.PriceAtTransaction,
                TransactionDate = trade.TransactionDate
            }).ToList();

            _tradeRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Trade, bool>>>()))
                                .ReturnsAsync(trades);
            _mapperMock.Setup(m => m.Map<IEnumerable<TransactionResponse>>(It.IsAny<IEnumerable<Trade>>()))
                       .Returns(new List<TransactionResponse>());

            // Act
            var result = await _service.GetTransactionsAsync(symbol);

            // Assert
            _tradeRepositoryMock.Verify(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Trade, bool>>>()), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<TransactionResponse>>(It.IsAny<IEnumerable<Trade>>()), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task BuyInstrumentAsync_LogsAndSavesTrade()
        {
            // Arrange
            var buyRequest = new BuyInstrumentRequest { Symbol = "AAPL" };
            var instrument = new InstrumentResponse { Symbol = "AAPL", CurrentPrice = 1 };
            var trade = new Trade();

            _marketDataProviderMock.Setup(m => m.FetchInstrumentAsync(buyRequest.Symbol)).ReturnsAsync(instrument);
            _mapperMock.Setup(m => m.Map<Trade>(buyRequest)).Returns(trade);

            // Act
            await _service.BuyInstrumentAsync(buyRequest);

            // Assert
            _tradeRepositoryMock.Verify(repo => repo.AddAsync(trade), Times.Once);
        }

        [Fact]
        public async Task SellInstrumentAsync_Succeeds_WhenSufficientHoldings()
        {
            // Arrange
            var sellRequest = new SellInstrumentRequest { Symbol = "AAPL", Quantity = 5 };
            var instrument = new InstrumentResponse { Symbol = "AAPL", CurrentPrice = 150.00m };
            var trade = new Trade();

            _marketDataProviderMock.Setup(m => m.FetchInstrumentAsync(sellRequest.Symbol)).ReturnsAsync(instrument);
            _tradeRepositoryMock.Setup(repo => repo.GetHoldingsAsync(sellRequest.Symbol)).ReturnsAsync(10);
            _mapperMock.Setup(m => m.Map<Trade>(sellRequest)).Returns(trade);

            // Act
            await _service.SellInstrumentAsync(sellRequest);

            // Assert
            _tradeRepositoryMock.Verify(repo => repo.AddTradeAsync(trade), Times.Once);
        }

        [Fact]
        public async Task SellInstrumentAsync_ThrowsException_WhenInsufficientHoldings()
        {
            // Arrange
            var sellRequest = new SellInstrumentRequest { Symbol = "AAPL", Quantity = 5 };

            _tradeRepositoryMock.Setup(repo => repo.GetHoldingsAsync(sellRequest.Symbol)).ReturnsAsync(3);
            
            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.SellInstrumentAsync(sellRequest));

            // Assert
            Assert.Equal("Insufficient shares to sell.", exception.Message);
        }
    }
}