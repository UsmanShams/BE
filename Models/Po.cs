using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Po
{
    public int PoId { get; set; }

    public int? VId { get; set; }

    public int? PId { get; set; }

    public DateTime? PoDate { get; set; }

    public int? PoQty { get; set; }

    public int? day { get; set; }

    public int? month { get; set; }

    public int? year { get; set; }

    public string? time { get; set; }
    public string? baseprltr { get; set; }

    public int? Count { get; set; }

    public int? PoUnique { get; set; }

    public int? PoPrice { get; set; }

    public virtual ICollection<Grn> Grns { get; set; } = new List<Grn>();

    public virtual Product? PIdNavigation { get; set; }

    public virtual Vender? VIdNavigation { get; set; }
}
