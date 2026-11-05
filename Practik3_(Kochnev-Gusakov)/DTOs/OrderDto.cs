using System.ComponentModel.DataAnnotations;

namespace Practik3__Kochnev_Gusakov_.DTOs
{
    // 1. Модель для создания заказа (присылает клиент)
    public class CreateOrderDto
    {
        [Required]
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }

    // Позиция внутри создаваемого заказа
    public class CreateOrderItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int SizeId { get; set; } // Какой именно размер покупают

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Количество товара в заказе должно быть больше 0.")]
        public int Quantity { get; set; }
    }

    // 2. Модель для отображения информации о заказе (отдаем клиенту)
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = null!;
        public int CustomerId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    // Позиция товара внутри готового заказа
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int SizeId { get; set; }
        public string SizeLabel { get; set; } = null!; // Например: "M" или "48"
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; } // Зафиксированная цена на момент покупки
    }

    // 3. Модель для смены статуса заказа менеджером
    public class UpdateOrderStatusDto
    {
        [Required]
        public string Status { get; set; } = null!; // Например: "Processing", "Shipped", "Cancelled"
    }
}
