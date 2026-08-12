using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Vender
{
    public int VId { get; set; }

    public string? VName { get; set; }

    public string? VEmail { get; set; }

    public string? VPhone { get; set; }

    public string? VNtn { get; set; }

    public virtual ICollection<Po> Pos { get; set; } = new List<Po>();
}
