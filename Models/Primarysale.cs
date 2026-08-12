using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Primarysale
{
    public int PsId { get; set; }

    public string? VName { get; set; }

    public string? PName { get; set; }

    public string? DcNo { get; set; }
	public string? Price { get; set; }
	public string? Total { get; set; }

	public int? PsPrice { get; set; }
    public DateTime? date { get; set; }

    public int? PsQty { get; set; }
    public int? OrderID { get; set; }
    public int? PsPack { get; set; }
    public int? day { get; set; }
    public int? year { get; set; }
    public string? month { get; set; }

    public string? time { get; set; }
    public DateTime? PsDate { get; set; }
}
