using System.ComponentModel.DataAnnotations;

namespace Practik3__Kochnev_Gusakov_.DTOs
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
        public string LetterSize { get; set; } = null!;
        public string? NumericSize { get; set; } // Исправлено: изменен тип на string?, так как в БД это строка
        public int StockQuantity { get; set; }
    }

}
