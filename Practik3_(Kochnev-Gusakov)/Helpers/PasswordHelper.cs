using BCrypt.Net;

namespace Practik3__Kochnev_Gusakov_.Helpers
{
    public class PasswordHelper
    {
       public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("Пароль не может быть пустым", nameof(password));
            }

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword (string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace (password) || string.IsNullOrWhiteSpace (hashedPassword))
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
