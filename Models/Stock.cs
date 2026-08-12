using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Stock
{
    public int SId { get; set; }
    public int PId { get; set; }

    public string? PName { get; set; }

    public int? PPack { get; set; }

    public int? SQty { get; set; }
}
