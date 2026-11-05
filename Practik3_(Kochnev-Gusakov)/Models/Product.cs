using System;
using System.Collections.Generic;

namespace Practik3__Kochnev_Gusakov_.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public int? PurchaseCount { get; set; }

    public string? ClothingType { get; set; }

    public bool? IsAvailable { get; set; }

    public virtual ICollection<ClothingSize> ClothingSizes { get; set; } = new List<ClothingSize>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
