using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Dc
{
    public int DcId { get; set; }

    public int? SsOrderno { get; set; }

    public DateTime? DcDate { get; set; }
}
