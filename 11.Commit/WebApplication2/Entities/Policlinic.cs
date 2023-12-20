using System.ComponentModel.DataAnnotations;
using WebApplication2.Controllers;

namespace WebApplication2.Entities
{
    public class Policlinic
    {
        public int PoliclinicID { get; set; }
        [Required]
        public string PolAd { get; set; }
        public ICollection<Doctor> Doctors { get; set; } //Polikinliklere ait doktorlara ulaşmak için.
    }
}
