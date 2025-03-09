using Application.DTOs;
using Domain.DTOs;
using Domain.Entities;

namespace Web.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync(string token);
        Task<IEnumerable<ProductWithCategoryDto>> GetAllProductWithCategoryAsync(string token);
        Task<ProductDto> GetProductByIdAsync(int id, string token);
        Task<bool> CreateProductAsync(ProductDto product, string token);
        Task<bool> UpdateProductAsync(int id, ProductDto product, string token);
        Task<bool> DeleteProductAsync(int id, string token);
       
    }
}
