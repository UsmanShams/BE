using System;
using System.Collections.Generic;

namespace be.Models;

public partial class looseledger
{
    public int LId { get; set; }

    public String? CName { get; set; }

    public string? Description { get; set; }
    public int? Qty { get; set; }

    public int? lIn { get; set; }
    public int? lOut { get; set; }
    public int? lBalance { get; set; }

    public int? day { get; set; }
    public int? month { get; set; }
    public int? year { get; set; }
    public string? time { get; set; }
    public string? type { get; set; }
}
