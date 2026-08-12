using System;
using System.Collections.Generic;

namespace be.Models;

public partial class cheque
{
    public int Ch_Id { get; set; }
    public int? CName { get; set; }

    public string? Description { get; set; }

    public string? Cheque_of { get; set; }
    public int? Amount { get; set; }
    public int? Status { get; set; }
    public int? Day { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
    public int? Pdc_Day { get; set; }
    public int? Pdc_Month { get; set; }
    public int? Pdc_Year { get; set; }
    public string? Time { get; set; }
    public string? Transfer_to { get; set; }
    public string? Type { get; set; }
}
