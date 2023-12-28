using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Entities;

namespace WebApplication2.Controllers
{

    [Authorize]
    public class RandevuController : Controller
    {
        private readonly DatabaseContext _context;

        public RandevuController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Appointment()
        {
            var poliklinikler = _context.Policlinics.ToList();
            ViewBag.Poliklinikler = new SelectList(poliklinikler, "PoliclinicID", "PolAd");

            return View();
        }

        public IActionResult GetDoktorlar(int PoliclinicID)
        {
            var doktorlar = _context.Doctors
            .Where(d => d.PoliclinicID == PoliclinicID)
            .Select(d => new { ID = d.DoctorID, Name = d.DoctorName })
            .ToList();
            return Json(doktorlar);
        }

        public IActionResult RandevuAl()
        {
            //var m = _context.Randevular.ToList();
            //return View(m);
            var randevular = _context.Randevular
            .Include(r => r.Doctor)
            .Include(r => r.Policlinic)
            .ToList();

            return View(randevular);
        }

        [HttpPost]
        public IActionResult RandevuAl(Randevu y)
        {
            if (ModelState.IsValid)
            {
                _context.Add(y);
                _context.SaveChanges();
                var randevular = _context.Randevular
                .Include(r => r.Doctor)
                .Include(r => r.Policlinic)
                .ToList();
                return View(randevular);//RedirectToAction, bir controller içindeki başka bir action'a yönlendirme yapmak için kullanılır.

            }
            else
            {
                ViewBag.msj = "Randevu Alınamadı !!!";
                return View("Appointment");
            }
        }


        

    }
}
