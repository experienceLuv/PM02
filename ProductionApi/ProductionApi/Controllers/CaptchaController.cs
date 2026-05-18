using Microsoft.AspNetCore.Mvc;
using ProductionApi.Services;

[ApiController]
[Route("api/captcha")]
public class CaptchaController : ControllerBase
{
    private readonly CaptchaService _captcha;
    public CaptchaController(CaptchaService captcha) => _captcha = captcha;

    [HttpGet]
    public IActionResult Get()
    {
        var (image, key) = _captcha.Generate();
        // Используем индексатор, чтобы не вылететь при повторном добавлении
        Response.Headers["X-Captcha-Key"] = key;
        return File(image, "image/png");
    }
}