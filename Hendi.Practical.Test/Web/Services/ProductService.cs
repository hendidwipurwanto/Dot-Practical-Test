using Application.DTOs;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;

namespace Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private string baseApi = "https://localhost:7066/api/Product";

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ProductDto>> GetProductAsync(string token)
        {
            var list = new List<ProductDto>();

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
                var response = await httpClient.GetAsync(baseApi);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Request failed: {response.StatusCode} - {errorResponse}");
                }

                // Deserialize response
                var res = await response.Content.ReadAsStringAsync();
                list = JsonSerializer.Deserialize<List<ProductDto>>(res, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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

            return list ?? new List<ProductDto>();
        }

        public Task<ProductDto> GetProductByIdAsync(string token, int id)
        {
            throw new NotImplementedException();
        }
    }
}
