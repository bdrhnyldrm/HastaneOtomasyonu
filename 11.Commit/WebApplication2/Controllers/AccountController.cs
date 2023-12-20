using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;
using System.ComponentModel.DataAnnotations;
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
				string hashedPassword = DoMD5HashedString(model.Password);

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

					return RedirectToAction("Appointment", "Randevu");


				}
				else
				{
					ModelState.AddModelError("", "Username or password is incorrect.");
				}
			}

			return View(model);
		}

		private string DoMD5HashedString(string s)
		{
			string md5salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
			string salted = s + md5salt;
			string hashed = salted.MD5();
			return hashed;
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

				string hashedPassword = DoMD5HashedString(model.Password);

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
			ProfileInfoLoader();

			return View();
		}

		private void ProfileInfoLoader()
		{
			Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
			User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

			ViewData["FullName"] = user.FullName;
			ViewData["ProfileImage"] = user.ProfileImageFileName;
		}

		[HttpPost]
		public IActionResult ProfileChangeFullName([Required][StringLength(50)]string? fullname)
		{
			if(ModelState.IsValid)//Modelstate ısvalid olayı requiredları kontrol eder.
			{
				Guid userid= new Guid(	User.FindFirstValue(ClaimTypes.NameIdentifier));

				User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

				user.FullName = fullname;
				_databaseContext.SaveChanges();

				return  RedirectToAction(nameof(Profile));
			}
			ProfileInfoLoader();
			return View("Profile");
		}

		[HttpPost]
		public IActionResult ProfileChangePassword([Required][MinLength(6)][MaxLength(16)] string? password)
		{
			if (ModelState.IsValid)//Modelstate ısvalid olayı requiredları kontrol eder.
			{
				Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

				User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

				string hashedPassword = DoMD5HashedString(password);

				user.Password = hashedPassword;
				_databaseContext.SaveChanges();

				ViewData["result"] = "PasswordChanged";
			}
			ProfileInfoLoader();
			return View("Profile");
		}

		[HttpPost]
		public IActionResult ProfileChangeImage([Required] IFormFile file)
		{
			if (ModelState.IsValid)//Modelstate ısvalid olayı requiredları kontrol eder.
			{
				Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

				User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

				string fileName = $"p_{userid}.jpg";
				Stream stream = new FileStream($"wwwroot/uploads/{fileName}", FileMode.OpenOrCreate); // Idye göre fotoğraf getirme işlemini yapıyoruz.

				file.CopyTo(stream);
				stream.Close();
				stream.Dispose();

				user.ProfileImageFileName = fileName;
				_databaseContext.SaveChanges();

				return RedirectToAction(nameof(Profile));
			}
			ProfileInfoLoader();
			return View("Profile");
		}

		public IActionResult Logout()
        {
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction(nameof(Login));
        }
    }
}
