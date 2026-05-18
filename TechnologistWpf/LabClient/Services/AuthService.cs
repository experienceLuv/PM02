using System;
using System.Threading.Tasks;
using LabClient.Models;

namespace LabClient.Services
{
    public static class AuthService
    {
        private static ApiClient Api => ApiClient.Instance;

        public static async Task<(byte[] Image, string Key)> GetCaptchaAsync()
        {
            var response = await Api.GetAsyncRaw("/api/captcha");
            var image = await response.Content.ReadAsByteArrayAsync();
            if (response.Headers.TryGetValues("X-Captcha-Key", out var values))
            {
                string key = string.Join("", values);
                return (image, key);
            }
            throw new Exception("Не найден заголовок X-Captcha-Key");
        }

        public static async Task<LoginResponse> LoginAsync(string login, string password, string captchaKey, string captchaText)
        {
            var dto = new
            {
                Login = login,
                Password = password,
                CaptchaKey = captchaKey,
                CaptchaText = captchaText
            };
            return await Api.PostAsync<LoginResponse>("/api/auth/login-with-captcha", dto);
        }
    }
}