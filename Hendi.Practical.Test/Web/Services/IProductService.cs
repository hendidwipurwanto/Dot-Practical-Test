using Application.DTOs;

namespace Web.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProductAsync(string token);
        Task<ProductDto> GetProductByIdAsync(string token, int id);
    }
}
