using Application.DTOs;
using Domain.Entities;

namespace Web.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(string token);
        Task<CategoryDto> GetCategoryByIdAsync(int id, string token);
        Task<bool> CreateCategoryAsync(CategoryDto category, string token);
        Task<bool> UpdateCategoryAsync(int id, CategoryDto category, string token);
        Task<bool> DeleteCategoryAsync(int id, string token);
    }
}
