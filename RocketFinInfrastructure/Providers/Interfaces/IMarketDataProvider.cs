using RocketFinDomain;

namespace RocketFinInfrastructure.Providers.Interfaces
{
    public interface IMarketDataProvider
    {
        Task<InstrumentResponse> FetchInstrumentAsync(string tickerSymbol);
        Task<IEnumerable<InstrumentResponse>> FetchInstrumentsAsync(string tickerSymbol);

    }
}
