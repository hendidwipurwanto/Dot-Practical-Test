using Application.DTOs;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using Domain.DTOs;
using Domain.Entities;
using System.Text;
namespace Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7066/api/Product";

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private void AddAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(string token)
        {
            AddAuthorizationHeader(token);
            var response = await _httpClient.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<ProductDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }



        public async Task<ProductDto> GetProductByIdAsync(int id, string token)
        {
            AddAuthorizationHeader(token);
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> CreateProductAsync(ProductDto product, string token)
        {
            AddAuthorizationHeader(token);
            var json = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BaseUrl, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProductAsync(int id, ProductDto product, string token)
        {
            AddAuthorizationHeader(token);
            var json = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{BaseUrl}/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductAsync(int id, string token)
        {
            AddAuthorizationHeader(token);
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<ProductWithCategoryDto>> GetAllProductWithCategoryAsync(string token)
        {

            //var response = await _httpClient.GetAsync("https://localhost:7066/api/Product/GetAllProductWithCategory");
            var list = new List<ProductWithCategoryDto>();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token tidak ditemukan di session.");
            }

            try
            {
                // Pastikan TLS yang aman digunakan (TLS 1.2 atau lebih tinggi)
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

                // Konfigurasi HttpClientHandler untuk koneksi yang lebih stabil
                var handler = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    AllowAutoRedirect = true,
                    UseCookies = true
                };

                using var httpClient = new HttpClient(handler)
                {
                    Timeout = TimeSpan.FromSeconds(100) // Set timeout 100 detik
                };

                // Set Authorization Token
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpClient.DefaultRequestHeaders.ConnectionClose = false;
                httpClient.DefaultRequestHeaders.Add("Connection", "Keep-Alive");

                // Kirim request ke API
                var response = await httpClient.GetAsync("https://localhost:7066/api/Product/GetAllProductWithCategory");

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Request failed: {response.StatusCode} - {errorResponse}");
                }

                // Deserialize response
                var res = await response.Content.ReadAsStringAsync();
                list = JsonSerializer.Deserialize<List<ProductWithCategoryDto>>(res, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                throw;
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine("Request Timeout: " + ex.Message);
                throw new TimeoutException("Request timeout terjadi, pastikan API bisa diakses.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }

            return list ?? new List<ProductWithCategoryDto>();
        }
    }
}
