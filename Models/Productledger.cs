using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Productledger
{
    public int PlId { get; set; }

    public string? CName { get; set; }

    public DateTime? PlDate { get; set; }

    public int? Ss_id { get; set; }

    public int? day { get; set; }
    public int? year { get; set; }
    public int? pono { get; set; }
    public string? Type { get; set; }

    public string? time { get; set; }

    public int? PlIn { get; set; }
    public int? Pid { get; set; }
    public int? PlOut { get; set; }
    public string? month { get; set; }
    public int? PlBalance { get; set; }
}
