using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly ISpecificRepository _specificRepository;
        private readonly IMemoryCache _cache;
        private readonly string productCacheKey = "productList";
        public ProductService(IGenericRepository<Product> productRepository, IMemoryCache cache, ISpecificRepository specificRepository)
        {
            _productRepository = productRepository;
            _cache = cache;
            _specificRepository = specificRepository;
        }

        public async Task<IEnumerable<ProductWithCategoryDto>> GetAllProductWithCategoryAsync()
        {

            if (!_cache.TryGetValue(productCacheKey, out IEnumerable<ProductWithCategoryDto> productwithcategory))
            {
                productwithcategory = await _specificRepository.GetAllProductWithCategoryAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))  // Reset waktu kalau ada akses
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); // Maksimal cache 10 menit

                _cache.Set(productCacheKey, productwithcategory, cacheOptions);
            }


            return productwithcategory;
        }


        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
           
             var   products = await _productRepository.GetAllAsync();

              
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
                    _cache.Set(cacheKey, product, TimeSpan.FromMinutes(10));
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
            _cache.Remove($"category_{product.Id}"); // Hapus cache kategori spesifik
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
            _cache.Remove(productCacheKey);
            _cache.Remove($"product_{id}");
        }


    }
}
