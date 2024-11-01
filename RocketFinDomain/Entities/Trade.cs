namespace RocketFinDomain.Entities
{
    public class Trade
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal PriceAtTransaction { get; set; }
        public DateTime TransactionDate { get; set; }
        public TradeType TradeType { get; set; }
    }

    public enum TradeType
    {
        Buy,
        Sell
    }

}
