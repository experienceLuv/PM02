public class LoginWithCaptchaDto
{
    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
    public string CaptchaKey { get; set; } = "";
    public string CaptchaText { get; set; } = "";
}