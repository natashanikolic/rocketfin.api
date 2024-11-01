using RocketFinDomain;

namespace RocketFinApp.Services.Interfaces
{
    public interface IInstrumentService
    {
        Task<InstrumentResponse> GetInstrumentDetailsAsync(string tickerSymbol);
        Task<IEnumerable<InstrumentResponse>> GetInstrumentsAsync(string tickerSymbol);
    }
}
