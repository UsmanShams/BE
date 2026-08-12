using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Secondarysale
{
    public int SsId { get; set; }

    public int? PId { get; set; }

    public int? CId { get; set; }

    public int? SsQty { get; set; }
    public DateTime? date { get; set; }

    public int? Status { get; set; }
    public int? SsOrderno { get; set; }
    public int? Day { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
    public string? Time { get; set; }
    public int? Price { get; set; }
    public int? TPrice { get; set; }
    public int? Pr_Price { get; set; }
    public string? Pack { get; set; }
    public string? Type { get; set; }
    public virtual Customer? CIdNavigation { get; set; }

    public virtual Product? PIdNavigation { get; set; }
}
