using Microsoft.EntityFrameworkCore;
using Practik3__Kochnev_Gusakov_.DTOs;
using Practik3__Kochnev_Gusakov_.Models;

namespace Practik3__Kochnev_Gusakov_.Services
{
    public class OrdersService
    {
        private readonly Pr3MagazContext _context;

        public OrdersService(Pr3MagazContext context)
        {
            _context = context;
        }

        // 1. Создание нового заказа с проверкой остатков
        public async Task<OrderDto?> CreateOrderAsync(int customerId, CreateOrderDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = new Order
                {
                    CustomerId = customerId,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Pending"
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                var orderDtoItems = new List<OrderItemDto>();

                foreach (var itemDto in dto.Items)
                {
                    var product = await _context.Products.FindAsync(itemDto.ProductId);
                    if (product == null || product.IsAvailable != true)
                        throw new Exception($"Товар с ID {itemDto.ProductId} недоступен.");

                    var size = await _context.ClothingSizes
                        .FirstOrDefaultAsync(s => s.Id == itemDto.SizeId && s.ProductId == itemDto.ProductId);

                    if (size == null || size.StockQuantity < itemDto.Quantity)
                        throw new Exception($"Недостаточно товара на складе.");

                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = itemDto.ProductId,
                        SizeId = itemDto.SizeId,
                        Quantity = itemDto.Quantity
                    };

                    size.StockQuantity -= itemDto.Quantity;
                    product.PurchaseCount = (product.PurchaseCount ?? 0) + itemDto.Quantity;

                    _context.OrderItems.Add(orderItem);

                    orderDtoItems.Add(new OrderItemDto
                    {
                        Id = orderItem.Id,
                        ProductId = itemDto.ProductId,
                        ProductName = product.Name ?? "",
                        SizeId = itemDto.SizeId,
                        SizeLabel = size.LetterSize ?? "",
                        Quantity = itemDto.Quantity,
                        PriceAtPurchase = product.Price // В DTO цену отдаем из карточки товара
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new OrderDto
                {
                    Id = order.Id,
                    CreatedAt = order.CreatedAt ?? DateTime.UtcNow,
                    Status = order.Status ?? "Pending",
                    CustomerId = order.CustomerId ?? customerId,
                    Items = orderDtoItems
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return null;
            }
        }

        // 2. Получение заказов конкретного покупателя
        public async Task<List<OrderDto>> GetCustomerOrdersAsync(int customerId)
        {
            return await GetOrdersQuery()
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();
        }

        // 3. Получение заказов по статусу
        public async Task<List<OrderDto>> GetOrdersByStatusAsync(string status)
        {
            return await GetOrdersQuery()
                .Where(o => o.Status == status)
                .ToListAsync();
        }

        // 4. Изменение статуса заказа менеджером
        public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.Status = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }

        private IQueryable<OrderDto> GetOrdersQuery()
        {
            return _context.Orders
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CreatedAt = o.CreatedAt ?? DateTime.UtcNow,
                    Status = o.Status ?? "Pending",
                    CustomerId = o.CustomerId ?? 0,
                    Items = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        ProductId = oi.ProductId,
                        ProductName = oi.Product != null ? (oi.Product.Name ?? "") : "",
                        SizeId = oi.SizeId,
                        SizeLabel = oi.Size != null ? (oi.Size.LetterSize ?? "") : "",
                        Quantity = oi.Quantity,
                        PriceAtPurchase = oi.Product != null ? oi.Product.Price : 0m // Берем цену из связанного товара
                    }).ToList()
                });
        }
    }
}
