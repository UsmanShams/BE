using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Followup
{
    public int FuId { get; set; }

    public string? CName { get; set; }

    public DateTime? FuDate { get; set; }

    public string? FuDescription { get; set; }

    public string? FuEntered { get; set; }
}
