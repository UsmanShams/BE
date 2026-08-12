using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Customerledger
{
    public int ClId { get; set; }

    public string? CName { get; set; }

    public string? Description { get; set; }

    public int? Orderid { get; set; }

    public int? Ss_id { get; set; }
    public DateTime? date { get; set; }
    public int? day { get; set; }
    public int? month { get; set; }
    public int? year { get; set; }
    public string? type { get; set; }
    public int? Qty { get; set; }
    public string? time { get; set; }

    public int? ClIn { get; set; }
    public int? ClOut { get; set; }
    public int? ClBalance { get; set; }
}
