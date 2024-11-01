using RocketFinDomain.Responses;

namespace RocketFinApp.Services.Interfaces
{
    public interface IPortfolioService
    {
        Task<IEnumerable<PortfolioResponse>> GetPortfolioAsync(string symbol = null);
    }
}
