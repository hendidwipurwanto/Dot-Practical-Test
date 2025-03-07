using Application.DTOs;
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
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMemoryCache _cache;
        private readonly string categoryCacheKey = "categoryList";

        public CategoryService(IGenericRepository<Category> categoryRepository, IMemoryCache cache)
        {
            _categoryRepository = categoryRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            if (!_cache.TryGetValue(categoryCacheKey, out IEnumerable<Category> categories))
            {
                categories = await _categoryRepository.GetAllAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))  // Reset waktu kalau ada akses
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); // Maksimal cache 10 menit

                _cache.Set(categoryCacheKey, categories, cacheOptions);
            }
            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            string cacheKey = $"category_{id}";
            if (!_cache.TryGetValue(cacheKey, out Category category))
            {
                category = await _categoryRepository.GetByIdAsync(id);
                if (category != null)
                {
                    _cache.Set(cacheKey, category, TimeSpan.FromMinutes(10));
                }
            }
            return category;
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
            _cache.Remove(categoryCacheKey); // Hapus cache biar data terbaru ke-load
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _categoryRepository.UpdateAsync(category);
            _cache.Remove(categoryCacheKey);
            _cache.Remove($"category_{category.Id}"); // Hapus cache kategori spesifik
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
            _cache.Remove(categoryCacheKey);
            _cache.Remove($"category_{id}");
        }

        public Task<CategoryDto> GetCategoryByIdAsync(string token, int id)
        {
            throw new NotImplementedException();
        }
    }
}
