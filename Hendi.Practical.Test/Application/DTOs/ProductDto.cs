using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProductDto
    {
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Price should be greather or equal by 0.")]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
