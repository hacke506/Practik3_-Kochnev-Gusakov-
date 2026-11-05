using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Practik3__Kochnev_Gusakov_.DTOs;
using Practik3__Kochnev_Gusakov_.Services;
using System.Security.Claims;

namespace Practik3__Kochnev_Gusakov_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Заказы доступны только для авторизованных пользователей
    public class OrdersController : ControllerBase
    {
        private readonly OrdersService _ordersService;

        public OrdersController(OrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        // 1. POST: /api/orders — Создание нового заказа
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            // Извлекаем ID текущего пользователя из JWT-токена, который сделал Участник №1
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Пользователь не авторизован или токен некорректен.");
            }

            var order = await _ordersService.CreateOrderAsync(userId, dto);
            if (order == null)
            {
                return BadRequest("Не удалось оформить заказ. Проверьте наличие товаров на складе.");
            }

            return Ok(order);
        }

        // 2. GET: /api/orders/customer/{customerId} — Получение заказов конкретного покупателя
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<List<OrderDto>>> GetCustomerOrders(int customerId)
        {
            // Безопасность: обычный пользователь может смотреть ТОЛЬКО свои заказы.
            // Менеджер может смотреть заказы любого клиента.
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRoleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRoleClaim != "Manager" && userIdClaim != customerId.ToString())
            {
                return Forbid(); // Возвращаем 403 Forbidden, если обычный юзер лезет в чужие заказы
            }

            var orders = await _ordersService.GetCustomerOrdersAsync(customerId);
            return Ok(orders);
        }

        // 3. GET: /api/orders — Получение заказов по статусу (Только для Менеджера)
        [HttpGet]
        [Authorize(Roles = "Manager")] // Доступ строго для роли Manager
        public async Task<ActionResult<List<OrderDto>>> GetByStatus([FromQuery] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return BadRequest("Статус не может быть пустым.");
            }

            var orders = await _ordersService.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        // 4. PATCH: /api/orders/{id}/status — Изменение статуса заказа менеджером
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Manager")] // Доступ строго для роли Manager
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDto dto)
        {
            var result = await _ordersService.UpdateOrderStatusAsync(id, dto.Status);
            if (!result)
            {
                return NotFound("Заказ с указанным ID не найден.");
            }

            return Ok(new { message = $"Статус заказа успешно изменен на '{dto.Status}'." });
        }
    }
}
