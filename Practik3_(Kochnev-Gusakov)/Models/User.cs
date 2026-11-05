using System;
using System.Collections.Generic;

namespace Practik3__Kochnev_Gusakov_.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string Login { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
