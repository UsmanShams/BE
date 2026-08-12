using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Product
{
    public int PId { get; set; }

    public string? PName { get; set; }

    public int? PPack { get; set; }

    public string? PType { get; set; }
    public string? PCode { get; set; }
	public string? Trade_disc { get; set; }
	public int? PPr { get; set; }

    public int? PSp { get; set; }

    public virtual ICollection<Po> Pos { get; set; } = new List<Po>();

    public virtual ICollection<Secondarysale> Secondarysales { get; set; } = new List<Secondarysale>();
}
