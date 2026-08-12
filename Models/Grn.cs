using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Grn
{
    public int GrnId { get; set; }

    public int? PoId { get; set; }

    public DateTime? GrnDate { get; set; }
    public int? Pono { get; set; }
    public int? day { get; set; }

    public int? month { get; set; }

    public int? year { get; set; }

    public string? time { get; set; }

    public string? GrnDc { get; set; }
    public virtual Po? Po { get; set; }
}
