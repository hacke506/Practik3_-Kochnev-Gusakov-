using System.ComponentModel.DataAnnotations;

namespace Practik3__Kochnev_Gusakov_.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Введите логин")]
        public string Login { get; set; } = null!;

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; } = null!;
    }
}
