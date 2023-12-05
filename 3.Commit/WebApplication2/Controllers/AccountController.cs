using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

		public IActionResult TurLogin()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(LoginViewModel model)
		{
            if (ModelState.IsValid)
            {

            }

			return View(model);
		}

		[HttpPost]
		public IActionResult TurLogin(TurLoginViewModel model)
		{
			if (ModelState.IsValid)
			{

			}

			return View(model);
		}

		public IActionResult Register()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}
