using System.ComponentModel.DataAnnotations;

namespace Practik3__Kochnev_Gusakov_.DTOs
{
    // Класс, который принимает новое количество товара для склада
    public class UpdateStockDto
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Количество товара на складе не может быть отрицательным.")]
        public int NewQuantity { get; set; }
    }
}
