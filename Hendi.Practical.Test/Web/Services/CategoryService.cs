using Application.DTOs;
using Domain.Entities;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Web.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7066/api/Category";

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private void AddAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(string token)
        {
            AddAuthorizationHeader(token);
            var response = await _httpClient.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<CategoryDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id, string token)
        {
            AddAuthorizationHeader(token);
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CategoryDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> CreateCategoryAsync(CategoryDto category, string token)
        {
            AddAuthorizationHeader(token);
            var json = JsonSerializer.Serialize(category);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BaseUrl, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryDto category, string token)
        {
            AddAuthorizationHeader(token);
            var json = JsonSerializer.Serialize(category);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{BaseUrl}/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCategoryAsync(int id, string token)
        {
            AddAuthorizationHeader(token);
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
