using Microsoft.AspNetCore.Mvc;
using ProductionApi.DTOs;
using ProductionApi.Services;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    // private readonly CaptchaService _captcha;   // пока не нужен

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login-with-captcha")]
    public async Task<IActionResult> LoginWithCaptcha([FromBody] LoginWithCaptchaDto dto)
    {
        // Капча временно отключена
        var user = await _authService.ValidateUser(dto.Login, dto.Password);
        if (user == null)
            return Unauthorized(new { message = "Неверный логин или пароль" });

        var token = _authService.GenerateToken(user);
        return Ok(new { token, user.FullName, user.RoleId });
    }
}