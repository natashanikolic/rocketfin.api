using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using RocketFinDomain;
using RocketFinInfrastructure.Providers.Interfaces;

namespace RocketFinInfrastructure.Providers
{
    public class MarketDataProvider: IMarketDataProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private string _apiUrl;
        private string _apiKey;

        public MarketDataProvider(HttpClient httpClient, IMapper mapper, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _apiUrl = configuration["YahooFinanceApi:BaseUrl"];
            _apiKey = configuration["YahooFinanceApi:ApiKey"];
        }

        public async Task<InstrumentResponse> FetchInstrumentAsync(string tickerSymbol)
        {
            var quotes = await FetchAsync(tickerSymbol);
            var quote = quotes.FirstOrDefault();

            if (quote == null)
            {
                throw new Exception("Instrument data not found.");
            }

            return _mapper.Map<InstrumentResponse>(quote);
        }

        public async Task<IEnumerable<InstrumentResponse>> FetchInstrumentsAsync(string tickerSymbol)
        {
            var quotes = await FetchAsync(tickerSymbol);
            return _mapper.Map<IEnumerable<InstrumentResponse>>(quotes);
        }

        private async Task<List<Security>> FetchAsync(string tickerSymbol)
        {
            var requestUrl = $"{_apiUrl}&symbols={tickerSymbol}";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            if (!string.IsNullOrEmpty(_apiKey))
            {
                request.Headers.Add("X-API-KEY", _apiKey);
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var marketData = JsonSerializer.Deserialize<YahooFinanceResponse>(responseData);

            var quotes = marketData?.quoteResponse?.result?.ToList();
            if (quotes == null || !quotes.Any())
            {
                throw new Exception("Instrument data not found.");
            }

            return quotes;
        }
    }
}
