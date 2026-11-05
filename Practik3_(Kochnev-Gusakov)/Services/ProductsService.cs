using Microsoft.EntityFrameworkCore;
using Practik3__Kochnev_Gusakov_.DTOs;
using Practik3__Kochnev_Gusakov_.Models;

namespace Practik3_Kochnev_Gusakov.Services
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
    }
}
