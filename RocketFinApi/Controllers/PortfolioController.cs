using Microsoft.AspNetCore.Mvc;
using RocketFinApp.Services.Interfaces;
using RocketFinDomain;
using RocketFinDomain.Requests;
using RocketFinDomain.Responses;

namespace RocketFinApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IInstrumentService _instrumentService;
        private readonly ITradeService _tradeService;
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IInstrumentService instrumentService, ITradeService tradeService, IPortfolioService portfolioService)
        {
            _instrumentService = instrumentService;
            _tradeService = tradeService;
            _portfolioService = portfolioService;
        }

        [HttpGet("securities/{tickerSymbol}")]
        public async Task<ActionResult<IEnumerable<InstrumentResponse>>> GetAsync(string tickerSymbol) 
        {

            var result = await _instrumentService.GetInstrumentDetailsAsync(tickerSymbol);

            return Ok(result);
        }

        [HttpGet("securities")]
        public async Task<ActionResult<IEnumerable<InstrumentResponse>>> GetSecuritiesAsync([FromQuery] string tickerSymbol)
        {
            var result = await _instrumentService.GetInstrumentsAsync(tickerSymbol);
            return Ok(result);
        }


        [HttpPost("buy")]
        public async Task<ActionResult> BuyInstrument([FromBody] BuyInstrumentRequest request)
        {
            await _tradeService.BuyInstrumentAsync(request);
            return Ok();
        }

        [HttpPost("sell")]
        public async Task<ActionResult> SellInstrument([FromBody] SellInstrumentRequest request)
        {
            try
            {
                await _tradeService.SellInstrumentAsync(request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet("transactions/recent")]
        public async Task<ActionResult<IEnumerable<TransactionSummaryResponse>>> GetRecentTransactions()
        {
            var transactions = await _tradeService.GetRecentTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("transactions")]
        public async Task<ActionResult<IEnumerable<TransactionResponse>>> GetTransactions([FromQuery] string tickerSymbol = null)
        {
            var transactions = await _tradeService.GetTransactionsAsync(tickerSymbol);
            return Ok(transactions);
        }

        [HttpGet("positions")]
        public async Task<ActionResult<IEnumerable<PortfolioResponse>>> GetPortfolio([FromQuery] string tickerSymbol = null)
        {
            var portfolio = await _portfolioService.GetPortfolioAsync(tickerSymbol);
            return Ok(portfolio);
        }
    }
}
