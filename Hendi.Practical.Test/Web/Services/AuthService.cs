using Application.DTOs;
using Azure.Core;
using System.Text;
using System.Text.Json;

namespace Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private string baseApi = "https://localhost:7066/api/auth/";
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<string?> LoginAsync(AuthRequestDto dto)
        {
            var jsonContent = JsonSerializer.Serialize(dto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var apiurl = $"{baseApi}login";
            var response = await _httpClient.PostAsync(apiurl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                return $"Failed to login: {errorResponse}";
            }

          
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AuthResponseDto>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Token; 
        }

        public async Task<bool> RegisterUserAsync(RegisterRequestDto dto)
        {
            var jsonContent = JsonSerializer.Serialize(dto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var apiurl = string.Format("{0}{1}", baseApi, "register");
            var response = await _httpClient.PostAsync(apiurl, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {               
                return false;
            }
        }
    }
}
