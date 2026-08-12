using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Invoice
{
    public int InId { get; set; }

    public int? SsOrderno { get; set; }

    public DateTime? InDate { get; set; }
    public string? InDate1 { get; set; }
}
