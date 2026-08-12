using System;
using System.Collections.Generic;

namespace be.Models;

public partial class BankDetail
{
    public int BdId { get; set; }

    public int? BdName { get; set; }
    public DateTime? date { get; set; }

    public string? BdSender { get; set; }

    public int? day { get; set; }

    public int? pay_id { get; set; }
    public int? month { get; set; }
    public string? ven_cus { get; set; }
    public int? year { get; set; }
    public int? typ { get; set; }
    public string? time { get; set; }

    public int? BdIn { get; set; }
    public int? BdOut { get; set; }
    public int? BdBalance { get; set; }
}
