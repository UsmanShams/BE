using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Customer
{
    public int CId { get; set; }

    public string? CName { get; set; }

    public string? CEmail { get; set; }

    public string? CPhone { get; set; }

    public string? CAddress { get; set; }
    public string? auth_per { get; set; }
    public string? CStatus { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Secondarysale> Secondarysales { get; set; } = new List<Secondarysale>();
}
