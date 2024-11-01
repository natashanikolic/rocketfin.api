using RocketFinDomain.Requests;
using RocketFinDomain.Responses;

namespace RocketFinApp.Services.Interfaces
{
    public interface ITradeService
    {
        Task<IEnumerable<TransactionSummaryResponse>> GetRecentTransactionsAsync();
        Task<IEnumerable<TransactionResponse>> GetTransactionsAsync(string symbol = null);
        Task BuyInstrumentAsync(BuyInstrumentRequest request);
        Task SellInstrumentAsync(SellInstrumentRequest request);
    }
}
