using HealMe.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealMe.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;
        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var user = await _userService.Login(username, password);
                if (user != null)
                {
                    TempData["ToastType"] = "success";
                    TempData["ToastTitle"] = "Đăng nhập thành công";
                    TempData["ToastMessage"] = "Chào mừng bạn trở lại, " + user.Username + "!";
                    HttpContext.Session.SetString("UserId", user.UserId.ToString());
                    return RedirectToAction("Index", "Task");
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Thất bại";
                TempData["ToastMessage"] = ex.Message;
                return View();
            }
        }
    }
}
