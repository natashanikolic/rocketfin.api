namespace RocketFinDomain.Responses
{
    public class TransactionSummaryResponse
    {
        public string Transaction { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Operation { get; set; }

    }
}
