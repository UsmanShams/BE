using System;
using System.Collections.Generic;

namespace be.Models;

public partial class delivery
{
    public int Id { get; set; }
    public int? del_no { get; set; }
    public int? qty { get; set; }
    public int? p_id { get; set; }
}
