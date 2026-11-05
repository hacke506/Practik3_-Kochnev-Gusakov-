using System.ComponentModel.DataAnnotations;

namespace Practik3__Kochnev_Gusakov_.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Логин обязателен для заполнения")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Логен должен бвть от 3 до 50 символов")]
        public string Login { get; set; } = null!;

        [Required(ErrorMessage ="Пароль обязателен для заполнения ")]
        [StringLength(100, MinimumLength = 6, ErrorMessage ="Пароль не должен быть не менее 6 симвлов")]
        public string Password { get; set; } = null!;

        public string? Phone {  get; set; }
        public string? Address { get; set; }
    }
}
