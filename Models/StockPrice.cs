    using System;
using System.Collections.Generic;
namespace be.Models
{
    public class StockPrice
    {
        public int Id { get; set; }
        public int PId { get; set; }
        public int Qty { get; set; }
        public int Pack { get; set; }
        public int Price { get; set; }
        public int Pono { get; set; }
        public int Date { get; set; }
        public int Month { get; set; }
        public int Order_id { get; set; }
        public int? loose_id { get; set; }
        public int Year { get; set; }
        public string Status { get; set; }
    }
}
