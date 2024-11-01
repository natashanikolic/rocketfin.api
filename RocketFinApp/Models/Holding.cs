namespace RocketFinApp.Models
{
    public class Holding
    {
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalShares { get; set; }
        public decimal CostBasis { get; set; }
    }
}
