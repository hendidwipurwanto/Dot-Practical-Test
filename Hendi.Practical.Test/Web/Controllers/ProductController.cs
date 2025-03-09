
using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Web.Services;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductController(IProductService productService, IHttpContextAccessor httpContextAccessor, ICategoryService categoryService)
        {
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            _categoryService = categoryService;
        }
        // GET: ProductController
        public async Task<ActionResult> Index()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            var product = await _productService.GetAllProductWithCategoryAsync(token);

            return View(product);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public async Task<ActionResult> Create()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            var categories = await _categoryService.GetAllCategoriesAsync(token);
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();


            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateItem(ProductDto dto)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            var isSucceed = await _productService.CreateProductAsync(dto, token);
            if (isSucceed)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: ProductController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            var categories = await _categoryService.GetAllCategoriesAsync(token);
            var product = await _productService.GetProductByIdAsync(id, token);

            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId); // Set default value

            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ProductDto dto)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");

            var isSucceed = await _productService.UpdateProductAsync(id, dto, token);

            if (isSucceed)
            {
                return RedirectToAction("Index");
            }


            return View();
        }

        // GET: ProductController/Delete/5
        public async Task<ActionResult> GetDelete(int id)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            var product = await _productService.GetProductByIdAsync(id, token);
            

            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            var isSucceed = await _productService.DeleteProductAsync(id, token);
            if(isSucceed)
            {
                return RedirectToAction("Index");
            }



            return View();
        }
    }
}
