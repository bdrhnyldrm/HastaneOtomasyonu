using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;
using System.Security.Claims;
using WebApplication2.Entities;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
	[Authorize]
    public class AccountController : Controller
    {
		private readonly DatabaseContext _databaseContext;//Database işelmelerinde bu değişkeni kullarak işlemleri yapacağız.
		private readonly IConfiguration _configuration;

		public AccountController(DatabaseContext databaseContext, IConfiguration configuration)
		{
			_databaseContext = databaseContext;
			_configuration = configuration;
		}

		[AllowAnonymous]
		public IActionResult Login()
        {
            return View();
        }

		[AllowAnonymous]
		public IActionResult TurLogin()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult Login(LoginViewModel model)
		{
            if (ModelState.IsValid)
            {
				string md5salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
				string saltedPassword = model.Password + md5salt;
				string hashedPassword = saltedPassword.MD5();

				User user = _databaseContext.Users.FirstOrDefault(x => x.Username.ToLower() == model.Username.ToLower() && x.Password == hashedPassword);

				if (user != null) // Eşlenme sağlanmışsa ve userın iççi boş değilse login işlemlerini artık yapabiliriz.
				{
					if (user.Locked)
					{
						ModelState.AddModelError(nameof(model.Username), "User is locked.");
						return View(model);
					}

					List<Claim> claims = new List<Claim>();
					claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
					claims.Add(new Claim(ClaimTypes.Name, user.FullName ?? string.Empty));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));
                    claims.Add(new Claim("UserName", user.Username));

					ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

					ClaimsPrincipal principal = new ClaimsPrincipal(identity);
					HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

					return RedirectToAction("Index", "Home");


				}
				else
				{
					ModelState.AddModelError("", "Username or password is incorrect.");
				}
			}

			return View(model);
		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult TurLogin(TurLoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				
			}

			return View(model);
		}

        [AllowAnonymous]
		public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
		public IActionResult Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				if(_databaseContext.Users.Any(x => x.Username.ToLower() ==  model.Username.ToLower()))
				{
					ModelState.AddModelError(nameof(model.Username), "Username already exist.");//Username alanı altında gösterir.
					View(model);
				}

				string md5salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
				string saltedPassword = model.Password + md5salt;
				string hashedPassword = saltedPassword.MD5();

				User user = new User
				{
					Username = model.Username,
					Password = hashedPassword // Böylelikle database e paswordun şifrelenmiş halini bascağız.
				};

				_databaseContext.Users.Add(user);
				int affectedRowCount = _databaseContext.SaveChanges();

				if(affectedRowCount == 0)
				{
					ModelState.AddModelError("", "User can not be added.");
				}

				else
				{
					return RedirectToAction(nameof(Login));
				}
			}
			return View();
		}
		public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Logout()
        {
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction(nameof(Login));
        }
    }
}
