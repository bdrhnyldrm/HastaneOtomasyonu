using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [HttpPost]
        public IActionResult RandevuAl(Randevu y)
        {
            if (ModelState.IsValid)
            {
                _context.Add(y);
                _context.SaveChanges();
                return RedirectToAction();//RANDEVU AL VİEWINE GÖNDERDİNMİ HATA VERİYOR BURAYI ÇÖZMEN LAZIM.

            }
            else
            {
                ViewBag.msj = "Randevu Alınamadı !!!"; 
                return View("Appointment");
            }
        }
        
    }
}
