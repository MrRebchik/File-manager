using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FileManagerLibrary.Models;

namespace FileManagerClient.Services
{
    public interface IStorageProviderService
    {
        Task<IStorageable> GetStorageablesAsync(bool isDirectory, string path);
    }
    internal class StorageProviderService : IStorageProviderService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StorageProviderService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task<IStorageable> GetStorageablesAsync(bool isDirectory = true, string path = "C:")
        {
            var client = _httpClientFactory.CreateClient();
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5047/api/file");
            request.Headers.Add("fullPath", path);
            request.Headers.Add("IsDirectory", isDirectory.ToString());
            request.Headers.Add("Authorization", "Bearer " + AuthService.Token);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                IStorageable result;
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,                                                         
                };
                if (isDirectory) 
                    result = JsonSerializer.Deserialize<Directory>(json, options);
                else 
                    result = JsonSerializer.Deserialize<File>(json, options);
                return result;
            }

            return null;
        }
    }
}
