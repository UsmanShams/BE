using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Bankstatement
{
    public int BsId { get; set; }

    public string? Bankname { get; set; }

    public string? Accountno { get; set; }

    public string? Accounttitle { get; set; }

    public int? Balance { get; set; }
}
