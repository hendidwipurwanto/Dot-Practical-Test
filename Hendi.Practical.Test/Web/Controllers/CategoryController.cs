
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
            var result = await _categoryService.GetCategoriesAsync(token);


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
           isSucceed =  await _categoryService.CreateAsync(dto, token);

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
        public ActionResult Edit(int id)
        {
            return View();
        }
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoryDto dto, int id)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
          //  var dto = await _categoryService.GetCategoriesAsync(token, id);


           // return View(dto);
        } */

        // POST: CategoryController/Edit/5
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

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoryController/Delete/5
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
