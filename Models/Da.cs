using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Da
{
    public int DasId { get; set; }

    public DateTime? DasDate { get; set; }

    public string? DasDescrition { get; set; }
    public int order_id { get; set; }
    public string? DasType { get; set; }
    public string? ven_cus { get; set; }
    public string? year { get; set; }
    public string? month { get; set; }
    public string? day { get; set; }

    public string? typ1 { get; set; }
    public string? DasExpense { get; set; }

    public int? DasDeit { get; set; }

    public int? DasCredit { get; set; }

    public int? DasBalance { get; set; }
}
