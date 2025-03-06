using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;
        private readonly string productCacheKey = "productList";

        public ProductService(IGenericRepository<Product> productRepository, IMemoryCache cache)
        {
            _productRepository = productRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            if (!_cache.TryGetValue(productCacheKey, out IEnumerable<Product> products))
            {
                products = await _productRepository.GetAllAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)) 
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); 

                _cache.Set(productCacheKey, products, cacheOptions);
            }
            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            string cacheKey = $"product_{id}";
            if (!_cache.TryGetValue(cacheKey, out Product product))
            {
                product = await _productRepository.GetByIdAsync(id);
                if (product != null)
                {
                    _cache.Set(cacheKey, product, TimeSpan.FromMinutes(5));
                }
            }
            return product;
        }

        public async Task AddProductAsync(Product product)
        {

            await _productRepository.AddAsync(product);
            _cache.Remove(productCacheKey); 
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
            _cache.Remove(productCacheKey);
            _cache.Remove($"product_{product.Id}"); 
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
            _cache.Remove(productCacheKey);
            _cache.Remove($"product_{id}");
        }
    }

}
