using System.ComponentModel.DataAnnotations;

namespace Practik3__Kochnev_Gusakov.DTOs
{
    // Класс для отправки информации о товаре клиенту
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? ClothingType { get; set; }
        public int PurchaseCount { get; set; }
        public bool IsAvailable { get; set; }

        // Список размеров, которые есть у этого товара
        public List<SizeDto> Sizes { get; set; } = new();
    }

    // Вспомогательный класс для отображения размеров конкретного товара
    public class SizeDto
    {
        public int Id { get; set; }
        public string LetterSize { get; set; } = null!; // Например: "M", "L", "XL"
        public int? NumericSize { get; set; }          // Например: 48, 50
        public int StockQuantity { get; set; }         // Сколько штук осталось на складе
    }
}
