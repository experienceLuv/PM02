using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LabClient.Services
{
    public class ApiClient
    {
        private readonly HttpClient _http;
        private static ApiClient _instance;
        public static ApiClient Instance => _instance ?? throw new InvalidOperationException("ApiClient не инициализирован.");

        public ApiClient(string baseUrl)
        {
            _http = new HttpClient { BaseAddress = new Uri(baseUrl) };
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _instance = this;
        }

        public void SetToken(string token)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<HttpResponseMessage> GetAsyncRaw(string url)
        {
            return await _http.GetAsync(url);
        }

        public async Task<byte[]> GetBytesAsync(string url)
        {
            var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<T> PostAsync<T>(string url, object data = null)
        {
            var jsonData = data == null ? null : JsonConvert.SerializeObject(data);
            var content = jsonData != null ? new StringContent(jsonData, Encoding.UTF8, "application/json") : null;
            var response = await _http.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task PostAsync(string url, object data = null)
        {
            var jsonData = data == null ? null : JsonConvert.SerializeObject(data);
            var content = jsonData != null ? new StringContent(jsonData, Encoding.UTF8, "application/json") : null;
            var response = await _http.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task PutAsync(string url, object data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _http.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string url)
        {
            var response = await _http.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}