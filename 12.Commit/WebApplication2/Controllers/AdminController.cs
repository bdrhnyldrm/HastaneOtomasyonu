using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [Authorize(Roles ="admin")] //Authorize ile birlikte sadece login olmuş olanlar girebilir.Verdiğimiz rolle birlikte artık admin olanlar sadece bu sayfayı açabilecek.
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
