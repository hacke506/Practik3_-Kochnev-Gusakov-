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
    }
}
