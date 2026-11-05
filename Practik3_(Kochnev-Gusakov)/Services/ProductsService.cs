using Microsoft.EntityFrameworkCore;
using Practik3__Kochnev_Gusakov_.DTOs;
using Practik3__Kochnev_Gusakov_.Models;

namespace Practik3__Kochnev_Gusakov_.Services
{
    public class ProductsService
    {
        private readonly Pr3MagazContext _context;

        public ProductsService(Pr3MagazContext context)
        {
            _context = context;
        }

        // 1. Получение списка всей доступной одежды
        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .Where(p => p.IsAvailable == true)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name ?? "",
                    Price = p.Price,
                    Description = p.Description,
                    ClothingType = p.ClothingType,
                    PurchaseCount = p.PurchaseCount ?? 0,
                    IsAvailable = p.IsAvailable == true,
                    Sizes = p.ClothingSizes.Select(s => new SizeDto
                    {
                        Id = s.Id,
                        LetterSize = s.LetterSize ?? "",
                        NumericSize = s.NumericSize, 
                        StockQuantity = s.StockQuantity ?? 0
                    }).ToList()
                })
                .ToListAsync();
        }

        // 2. Поиск товаров по части названия
        public async Task<List<ProductDto>> SearchProductsAsync(string search)
        {
            return await _context.Products
                .Where(p => p.IsAvailable == true && p.Name != null && p.Name.Contains(search))
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name ?? "",
                    Price = p.Price, 
                    Description = p.Description,
                    ClothingType = p.ClothingType,
                    PurchaseCount = p.PurchaseCount ?? 0,
                    IsAvailable = p.IsAvailable == true,
                    Sizes = p.ClothingSizes.Select(s => new SizeDto
                    {
                        Id = s.Id,
                        LetterSize = s.LetterSize ?? "",
                        NumericSize = s.NumericSize, 
                        StockQuantity = s.StockQuantity ?? 0
                    }).ToList()
                })
                .ToListAsync();
        }
        // 3. Изменение количества товара определённого размера менеджером
        public async Task<bool> UpdateSizeStockAsync(int productId, int sizeId, int newQuantity)
        {
            // Ищем нужный размер у конкретного товара
            var size = await _context.ClothingSizes
                .FirstOrDefaultAsync(s => s.Id == sizeId && s.ProductId == productId);

            if (size == null)
            {
                return false; // Если такой размер или товар не найдены
            }

            size.StockQuantity = newQuantity; // Обновляем остаток
            await _context.SaveChangesAsync();
            return true;
        }

        // 4. Убрать или вернуть товар в продажу (изменение доступности)
        public async Task<bool> ToggleProductAvailabilityAsync(int id, bool isAvailable)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false; // Товар не найден
            }

            product.IsAvailable = isAvailable; // Меняем флаг доступности в БД
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
