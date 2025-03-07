using Application.DTOs;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Web.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private string baseApi = "https://localhost:7066/api/category";
        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
                _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<bool> CreateAsync(CategoryDto category, string token)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("Token tidak ditemukan.");

            try
            {
                // Menambahkan Bearer Token ke Header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Menentukan URL API
                string apiUrl = "https://localhost:7066/api/category";

                // Serialize Data ke JSON
                var jsonContent = JsonSerializer.Serialize(category);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Mengirim Request POST
                var response = await _httpClient.PostAsync(apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Request failed: {response.StatusCode} - {errorResponse}");
                }

                return true; // Berhasil
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;  // Agar stack trace asli tidak hilang
            }
        }



        public async Task<List<CategoryDto>> GetCategoriesAsync(string token, int id)
        {
            var list = new List<CategoryDto>();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token not found in session.");
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
                list = JsonSerializer.Deserialize<List<CategoryDto>>(res, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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

            return list ?? new List<CategoryDto>();
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync(string token)
        {
            var list = new List<CategoryDto>();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token not found in session.");
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
                list = JsonSerializer.Deserialize<List<CategoryDto>>(res, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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

            return list ?? new List<CategoryDto>();
        }

        public Task<CategoryDto> GetCategoryById(string token, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(string token, int id)
        {
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("Token tidak ditemukan.");

            try
            {
                // Menambahkan Bearer Token ke Header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Menentukan URL API (dengan ID kategori)
                string apiUrl = $"https://localhost:7066/api/category/{id}";

                // Mengirim Request GET
                var response = await _httpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Request failed: {response.StatusCode} - {errorResponse}");
                }

                // Deserialize JSON ke `CategoryDto`
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var category = JsonSerializer.Deserialize<CategoryDto>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return category ?? throw new Exception("Data kategori tidak ditemukan.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }



    }
}
