using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using Web.Services;


namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(IHttpContextAccessor httpContextAccessor,IAuthService authService)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AuthRequestDto dto)
        {
            var token = await _authService.LoginAsync(dto);

            if (!string.IsNullOrEmpty(token))
            {
       
                _httpContextAccessor.HttpContext?.Session.SetString("token", token);

                // _httpContextAccessor.HttpContext?.Session.GetString("token");

                return RedirectToAction("Index", "Dashboard");
               
            }

            return View();
        }



        public ActionResult Login()
        {
           


            return View();
        }

        // GET: AccountController/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: AccountController/Create
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

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
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

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
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
