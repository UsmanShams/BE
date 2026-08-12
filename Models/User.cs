using System;
using System.Collections.Generic;

namespace be.Models;

public partial class User
{
    public int UId { get; set; }

    public string? UName { get; set; }

    public string? UEmail { get; set; }

    public string? UPhone { get; set; }

    public int? URole { get; set; }
    public string? Pass { get; set; }

}
