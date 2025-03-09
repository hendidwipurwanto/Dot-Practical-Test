using Domain.DTOs;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SpecificRepository :ISpecificRepository
    {
        private readonly ApplicationDbContext _context;
        public SpecificRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductWithCategoryDto>> GetAllProductWithCategoryAsync()
        {
            return await _context.Products
                .Include(p => p.Category) // Eager loading
                .Select(p => new ProductWithCategoryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name
                })
                .ToListAsync();
        }
    }
}
