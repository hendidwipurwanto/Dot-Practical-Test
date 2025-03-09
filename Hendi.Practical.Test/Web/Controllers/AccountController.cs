using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
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

        public ActionResult Login()
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

               

                return RedirectToAction("Index", "Dashboard");
               
            }

            return View();
        }

        public ActionResult Logout()
        {
            _httpContextAccessor.HttpContext?.Session.Remove("token");

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterRequestDto dto)
        {
            var isSucceed = await _authService.RegisterUserAsync(dto);

            if (isSucceed)
            {
                return RedirectToAction("Info");
            }

            return View();
        }

        public ActionResult Info()
        {
            return View();
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
