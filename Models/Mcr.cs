using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Mcr
{
    public int McrId { get; set; }

    public string? CName { get; set; }

    public DateTime? SupplyDate { get; set; }

    public int? Debit { get; set; }

    public int? Credit { get; set; }

    public int? Balance { get; set; }

    public string? McrStatus { get; set; }
}
