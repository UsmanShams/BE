using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Pay
{
    public int PaId { get; set; }

    public int? PayerName { get; set; }
    public string? Description { get; set; }
    public string? Mode { get; set; }
    public int? PAmount { get; set; }
    public int? Day { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
    public string? Time { get; set; }
    public string? Type { get; set; }
}
