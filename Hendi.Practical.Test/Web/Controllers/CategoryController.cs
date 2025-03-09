
using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Services;

namespace Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICategoryService _categoryService;

        public CategoryController(IHttpContextAccessor httpContextAccessor,ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: CategoryController
        public async Task<ActionResult> Index()
        {

            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            var result = await _categoryService.GetAllCategoriesAsync(token);


            return View(result);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateItem(CategoryDto dto)
        {
            bool isSucceed = false;
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
           isSucceed =  await _categoryService.CreateCategoryAsync(dto, token);

            if (isSucceed)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        // POST: CategoryController/Create
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

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            var model = await _categoryService.GetCategoryByIdAsync(id,token);

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoryDto dto, int id)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
          
            var isSucceed = await _categoryService.UpdateCategoryAsync(id,dto,token);
            if (isSucceed)
            {
                return RedirectToAction("index");
            }

            return View(dto);
        }

        public async Task<ActionResult> GetDelete(int id)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            var model = await _categoryService.GetCategoryByIdAsync(id, token);

            return View(model);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
           var isSucceed =  await _categoryService.DeleteCategoryAsync(id, token);

            if (isSucceed)
            {
                return RedirectToAction("index");
            }



            return View();
           
        }
    }
}
