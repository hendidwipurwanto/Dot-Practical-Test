using Application.DTOs;

namespace Web.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetCategoriesAsync(string token);
        Task<CategoryDto> GetCategoryById(string token, int id);

        Task<bool> CreateAsync(CategoryDto dto, string token);
    }
}
