using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
   // [Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _productService.GetProductsAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return product != null ? Ok(product) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto dto)
    {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Return error validation to frontend

            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                 CategoryId=dto.CategoryId
            };

            await _productService.AddProductAsync(product);
            return Ok(new { message = "Product created successfully!" });
        }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductDto dto)
    {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new { message = "Product not found!" });

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;

           await _productService.UpdateProductAsync(product);
            return Ok(new { message = "Product updated successfully!" });
        }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }
}

}
