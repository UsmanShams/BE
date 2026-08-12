using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Vendorledger
{
    public int VlId { get; set; }

    public string? VName { get; set; }

    public string? Description { get; set; }
    public string? Type { get; set; }

    public int? Orderid { get; set; }
    public DateTime? date { get; set; }
    public int? day { get; set; }
    public int? month { get; set; }
    public int? year { get; set; }

    public int? Qty { get; set; }
    public string? time { get; set; }

    public int? VlIn { get; set; }
    public int? VlOut { get; set; }
    public int? VlBalance { get; set; }
}
