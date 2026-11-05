namespace Practik3__Kochnev_Gusakov_.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!; // JWT-токен
        public int UserId { get; set; }
        public string Login { get; set; } = null!;
        public string Role { get; set; } = null!;

    }
}
