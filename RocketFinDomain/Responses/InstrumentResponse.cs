
namespace RocketFinDomain
{
    public class InstrumentResponse
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal ChangeValue { get; set; }
        public decimal ChangePercentage { get; set; }
    }

}