using System;
using System.Collections.Generic;

namespace be.Models;

public partial class cus_pro
{
    public int Id { get; set; }
    public int? pid { get; set; }
    public int? cid { get; set; }
    public int? baseprice { get; set; }
}
