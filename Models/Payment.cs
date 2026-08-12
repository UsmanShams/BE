using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Payment
{
    public int PayId { get; set; }

    public int? CId { get; set; }

    public int? PayAmount { get; set; }

    public string? PayType { get; set; }

    public string? PayTo { get; set; }

    public DateTime? PayDate { get; set; }

    public string? PayDescription { get; set; }

    public virtual Customer? CIdNavigation { get; set; }
}
