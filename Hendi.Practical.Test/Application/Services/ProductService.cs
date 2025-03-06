using Domain.Entities;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;

        public ProductService(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync() => await _productRepository.GetAllAsync();

        public async Task<Product> GetProductByIdAsync(int id) => await _productRepository.GetByIdAsync(id);

        public async Task AddProductAsync(Product product) => await _productRepository.AddAsync(product);

        public async Task UpdateProductAsync(Product product) => await _productRepository.UpdateAsync(product);

        public async Task DeleteProductAsync(int id) => await _productRepository.DeleteAsync(id);
    }

}
