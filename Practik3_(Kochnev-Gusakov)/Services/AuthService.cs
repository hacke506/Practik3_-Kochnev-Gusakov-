using Practik3__Kochnev_Gusakov_.DTOs;
using Practik3__Kochnev_Gusakov_.Helpers;
using Practik3__Kochnev_Gusakov_.Models;
using Microsoft.EntityFrameworkCore;


namespace Practik3__Kochnev_Gusakov_.Services
{
    public class RegistrationResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class AuthService
    {
        private readonly Pr3MagazContext _context;
        private readonly JwtService _jwtService;

        public AuthService(Pr3MagazContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }


        public async Task<RegistrationResult> RegisterAsync(RegisterDto dto)
        {
         
            var userExists = await _context.Users.AnyAsync(u => u.Login == dto.Login);
            if (userExists)
            {
                return new RegistrationResult
                {
                    Success = false,
                    ErrorMessage = "Пользователь с таким логином уже существует."
                };
            }

            
            string hashedPassword = PasswordHelper.HashPassword(dto.Password);

            
            var newUser = new User
            {
                Login = dto.Login,
                PasswordHash = hashedPassword,
                Phone = dto.Phone,
                Address = dto.Address,
                Role = "User" 
            };

            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return new RegistrationResult { Success = true };
            }
            catch (Exception ex)
            {
                
                return new RegistrationResult
                {
                    Success = false,
                    ErrorMessage = "Произошла ошибка при сохранении пользователя."
                };
            }
        }


        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == dto.Login);
            if (user == null)
            {
                return null; 
            }

            
            bool isPasswordValid = PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return null; 
            }

            
            var token = _jwtService.GenerateToken(user);

            
            return new AuthResponseDto
            {
                Token = token,
                UserId = user.Id,
                Login = user.Login,
                Role = user.Role
            };
        }
    }
}
