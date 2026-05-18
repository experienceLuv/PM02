using Microsoft.Extensions.Caching.Memory;
using System.Drawing;
using System.Drawing.Imaging;

namespace ProductionApi.Services
{
    public class CaptchaService
    {
        private readonly IMemoryCache _cache;
        public CaptchaService(IMemoryCache cache) => _cache = cache;

        public (byte[] Image, string Key) Generate()
        {
            var text = GenerateRandomText(5);
            var key = Guid.NewGuid().ToString();
            _cache.Set(key, text, TimeSpan.FromMinutes(5));

            using var bitmap = new Bitmap(200, 80);
            using var g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
            var font = new Font("Arial", 24, FontStyle.Bold);
            g.DrawString(text, font, Brushes.Black, new PointF(10, 10));
            // Добавим шума
            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                bitmap.SetPixel(random.Next(200), random.Next(80), Color.Gray);
            }
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return (ms.ToArray(), key);
        }

        public bool Validate(string key, string userInput)
        {
            if (_cache.TryGetValue(key, out string? correctText))
            {
                _cache.Remove(key); // одноразовая
                return string.Equals(correctText, userInput, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private string GenerateRandomText(int length)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var random = new Random();
            return new string(Enumerable.Range(0, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
        }
    }
}