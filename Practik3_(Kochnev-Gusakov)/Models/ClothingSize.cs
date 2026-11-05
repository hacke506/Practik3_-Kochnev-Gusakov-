using System;
using System.Collections.Generic;

namespace Practik3__Kochnev_Gusakov_.Models;

public partial class ClothingSize
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string? LetterSize { get; set; }

    public string? NumericSize { get; set; }

    public int? StockQuantity { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Product Product { get; set; } = null!;
}
