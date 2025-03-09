using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface ISpecificRepository
    {
        Task<IEnumerable<ProductWithCategoryDto>> GetAllProductWithCategoryAsync();
    }
}
