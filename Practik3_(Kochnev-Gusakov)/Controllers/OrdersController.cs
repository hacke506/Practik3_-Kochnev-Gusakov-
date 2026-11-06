using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Practik3__Kochnev_Gusakov_.Services;
using Practik3__Kochnev_Gusakov_.DTOs;
using System.Security.Claims;

namespace Practik3__Kochnev_Gusakov_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersService _ordersService;

        public OrdersController(OrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Пользователь не идентифицирован или токен некорректен.");
            }

            int customerId = int.Parse(userIdClaim.Value);

            var order = await _ordersService.CreateOrderAsync(customerId, dto);
            if (order == null)
            {
                return BadRequest("Не удалось оформить заказ. Проверьте остатки товаров на складе.");
            }

            return Ok(order);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<List<OrderDto>>> GetCustomerOrders(int customerId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaim = User.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null) return Unauthorized();

            int currentUserId = int.Parse(userIdClaim.Value);
            string currentRole = userRoleClaim?.Value ?? "User";

            if (currentUserId != customerId && currentRole != "Manager")
            {
                return Forbid();
            }

            var orders = await _ordersService.GetCustomerOrdersAsync(customerId);
            return Ok(orders);
        }

        [HttpGet("status")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<List<OrderDto>>> GetByStatus([FromQuery] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return BadRequest("Статус заказа не указан.");
            }

            var orders = await _ordersService.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Manager")]
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
