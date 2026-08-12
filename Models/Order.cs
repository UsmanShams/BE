using System;
using System.Collections.Generic;
namespace be.Models
{
    public class Order
    {
        public int OId { get; set; }

        public int? CId { get; set; }

        public int? PId { get; set; }
        public int? Qty { get; set; }
        public int? delivered { get; set; }

        public int? OrUnique { get; set; }
        public int? OPrice { get; set; }
        public int? Count { get; set; }

        public int? day { get; set; }

        public int? month { get; set; }

        public int? year { get; set; }

        public string? time { get; set; }
        public string? type { get; set; }
    }
}
