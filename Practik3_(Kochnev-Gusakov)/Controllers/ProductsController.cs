using Microsoft.AspNetCore.Mvc;
using Practik3__Kochnev_Gusakov_.DTOs;
using Practik3__Kochnev_Gusakov_.Services;

namespace Practik3__Kochnev_Gusakov_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsService _productsService;

        // Внедряем сервис через конструктор
        public ProductsController(ProductsService productsService)
        {
            _productsService = productsService;
        }

        // 1. GET: /api/products — Получение списка всей доступной одежды
        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAll()
        {
            var products = await _productsService.GetAllProductsAsync();
            return Ok(products);
        }

        // 2. GET: /api/products/search?query=shirt — Поиск товаров по части названия
        [HttpGet("search")]
        public async Task<ActionResult<List<ProductDto>>> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Поисковый запрос не может быть пустым.");
            }

            var products = await _productsService.SearchProductsAsync(query);
            return Ok(products);
        }

        // 3. PATCH: /api/products/{productId}/sizes/{sizeId}/stock — Изменение остатков менеджером
        [HttpPatch("{productId}/sizes/{sizeId}/stock")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Manager")] // Доступ только для Manager
        public async Task<IActionResult> UpdateStock(int productId, int sizeId, [FromBody] UpdateStockDto dto)
        {
            var result = await _productsService.UpdateSizeStockAsync(productId, sizeId, dto.NewQuantity);
            if (!result)
            {
                return NotFound("Товар или указанный размер не найдены в базе данных.");
            }

            return Ok(new { message = "Остатки на складе успешно обновлены." });
        }

        // 4. PATCH: /api/products/{id}/availability — Убрать или вернуть товар в продажу
        [HttpPatch("{id}/availability")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Manager")] // Доступ только для Manager
        public async Task<IActionResult> ToggleAvailability(int id, [FromQuery] bool isAvailable)
        {
            var result = await _productsService.ToggleProductAvailabilityAsync(id, isAvailable);
            if (!result)
            {
                return NotFound("Указанный товар не найден.");
            }

            string status = isAvailable ? "вернули в продажу" : "убрали из продажи";
            return Ok(new { message = $"Товар успешно {status}." });
        }
    }
}
