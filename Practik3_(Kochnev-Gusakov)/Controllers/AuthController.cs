using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practik3__Kochnev_Gusakov_.DTOs;
using Practik3__Kochnev_Gusakov_.Models;
using Practik3__Kochnev_Gusakov_.Services;

namespace Practik3__Kochnev_Gusakov_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // 1. POST: /api/auth/register — Регистрация нового покупателя
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(new { message = "Регистрация успешно завершена!" });
        }

        // 2. POST: /api/auth/login — Вход и выдача JWT-токена
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _authService.LoginAsync(dto);
            if (response == null)
            {
                return BadRequest("Неверный логин или пароль.");
            }

            return Ok(response);
        }
    }
}
