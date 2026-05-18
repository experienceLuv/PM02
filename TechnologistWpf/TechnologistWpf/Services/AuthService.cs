using System;
using System.Threading.Tasks;
using TechnologistWpf.Models;

namespace TechnologistWpf.Services
{
    public static class AuthService
    {
        private static ApiClient Api => ApiClient.Instance;

        public static async Task<(byte[] Image, string Key)> GetCaptchaAsync()
        {
            var response = await Api.GetAsyncRaw("/api/captcha");
            var image = await response.Content.ReadAsByteArrayAsync();
            if (response.Headers.TryGetValues("X-Captcha-Key", out var values))
                return (image, string.Join("", values));
            throw new Exception("Не найден заголовок X-Captcha-Key");
        }

        public static async Task<LoginResponse> LoginAsync(string login, string password, string captchaKey, string captchaText)
        {
            var dto = new { Login = login, Password = password, CaptchaKey = captchaKey, CaptchaText = captchaText };
            return await Api.PostAsync<LoginResponse>("/api/auth/login-with-captcha", dto);
        }
    }
}