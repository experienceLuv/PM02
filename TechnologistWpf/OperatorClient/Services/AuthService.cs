using System.Threading.Tasks;
using OperatorClient.Models;

namespace OperatorClient.Services
{
    public static class AuthService
    {
        private static ApiClient Api => ApiClient.Instance;

        public static async Task<LoginResponse> LoginAsync(string login, string password)
        {
            var dto = new { Login = login, Password = password, CaptchaKey = "", CaptchaText = "" };
            return await Api.PostAsync<LoginResponse>("/api/auth/login-with-captcha", dto);
        }
    }
}