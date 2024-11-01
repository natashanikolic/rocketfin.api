namespace RocketFinDomain.Responses
{
    public class PortfolioResponse
    {
        public string Symbol { get; set; }
        public decimal TotalShares { get; set; }
        public decimal CostBasis { get; set; }
        public decimal MarketValue { get; set; }
        public decimal UnrealizedReturnRate { get; set; }
        public decimal UnrealizedProfitLoss { get; set; }
    }
}
