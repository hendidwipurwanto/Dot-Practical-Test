using Application.DTOs;

using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Services;

namespace Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        public DashboardController(IHttpContextAccessor httpContextAccessor, ICategoryService categoryService, IProductService productService)
        {
            _httpContextAccessor = httpContextAccessor;
            _categoryService = categoryService;
            _productService = productService;
        }
        // GET: DashboardController
        public async Task<ActionResult> Index()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            try
            {
                var categories = await _categoryService.GetCategoriesAsync(token);
                ViewBag.Categories = categories.Count;

                var products = await _productService.GetProductAsync(token);
                ViewBag.products = products.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
         
            return View();
        }

        // GET: DashboardController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DashboardController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DashboardController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DashboardController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DashboardController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DashboardController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DashboardController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
