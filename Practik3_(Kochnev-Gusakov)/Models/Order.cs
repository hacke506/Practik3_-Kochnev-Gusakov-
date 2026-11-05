using System;
using System.Collections.Generic;

namespace Practik3__Kochnev_Gusakov_.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string Status { get; set; } = null!;

    public int? CustomerId { get; set; }

    public virtual User? Customer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
