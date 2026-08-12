using System;
using System.Collections.Generic;

namespace be.Models;

public partial class OrderNo
{
    public int OrdernoId { get; set; }

    public string? OrdernoStatus { get; set; }
    public int Customer { get; set; }
}
