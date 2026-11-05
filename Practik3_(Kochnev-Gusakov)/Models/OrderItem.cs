using System;
using System.Collections.Generic;

namespace Practik3__Kochnev_Gusakov_.Models;

public partial class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int SizeId { get; set; }

    public int Quantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual ClothingSize Size { get; set; } = null!;
}
