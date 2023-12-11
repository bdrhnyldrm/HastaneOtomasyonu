using Microsoft.AspNetCore.Mvc;
using WebApplication2.Entities;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class UserController : Controller
    {

		private readonly DatabaseContext _databaseContext;

		public UserController(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
		}

		public IActionResult Index()
        {
			List<User> users = _databaseContext.Users.ToList();
			List<UserModel> model = new List<UserModel>();

            return View();
        }
    }
}
