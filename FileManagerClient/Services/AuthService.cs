using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System;

namespace FileManagerClient.Services
{
    public interface IAuthService
    {
        public static string Token;
        public static string Username;
        Task<string> GetTokenAsync(string username, string password, bool isNew = false);
    }
    public class AuthService : IAuthService
    {
        public static string Token;
        public static string Username;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetTokenAsync(string username, string password, bool isNew = false)
        {
            string url = isNew ? "http://localhost:5047/api/auth/register" : "http://localhost:5047/api/auth/index";
            var client = _httpClientFactory.CreateClient();
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Username", username);
            request.Headers.Add("password", password);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string result = "";
                var json = await response.Content.ReadAsStringAsync();
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    JsonElement root = doc.RootElement;
                    result = root.GetProperty("access_token").GetString();
                }
                return result;
            }

            return null;
        }
    }
}
