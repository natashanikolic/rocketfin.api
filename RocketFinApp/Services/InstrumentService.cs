
using Microsoft.Extensions.Logging;
using RocketFinApp.Services.Interfaces;
using RocketFinDomain;
using RocketFinInfrastructure.Providers.Interfaces;

namespace RocketFinApp.Services
{
    public class InstrumentService: IInstrumentService
    {
        private readonly IMarketDataProvider _marketDataProvider;
        private readonly ILogger<InstrumentService> _logger;

        public InstrumentService(IMarketDataProvider marketDataProvider, ILogger<InstrumentService> logger)
        {
            _marketDataProvider = marketDataProvider;
            _logger = logger;
        }

        public async Task<InstrumentResponse> GetInstrumentDetailsAsync(string ticker)
        {
            _logger.LogInformation("Executing GetInstrumentDetailsAsync ${ticker}", ticker);
            return await _marketDataProvider.FetchInstrumentAsync(ticker);

        }

        public async Task<IEnumerable<InstrumentResponse>> GetInstrumentsAsync(string ticker)
        {
            _logger.LogInformation("Executing GetInstrumentsAsync ${ticker}", ticker);
            return await _marketDataProvider.FetchInstrumentsAsync(ticker);
        }
    }
}
