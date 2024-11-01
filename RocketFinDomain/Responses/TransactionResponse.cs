using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketFinDomain.Responses
{
    public class TransactionResponse
    {
        public string Symbol { get; set; }
        public string Operation { get; set; }
        public decimal Quantity { get; set; }
        public decimal PriceAtTransaction { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
