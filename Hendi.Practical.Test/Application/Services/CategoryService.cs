using Domain.Entities;
using Infrastructure.Repositories;
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

        public CategoryService(IGenericRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync() => await _categoryRepository.GetAllAsync();

        public async Task<Category> GetCategoryByIdAsync(int id) => await _categoryRepository.GetByIdAsync(id);

        public async Task AddCategoryAsync(Category category) => await _categoryRepository.AddAsync(category);

        public async Task UpdateCategoryAsync(Category category) => await _categoryRepository.UpdateAsync(category);

        public async Task DeleteCategoryAsync(int id) => await _categoryRepository.DeleteAsync(id);
    }

}
