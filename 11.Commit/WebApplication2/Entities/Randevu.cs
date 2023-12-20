

namespace WebApplication2.Entities
{
    public class Randevu
    {
        public int RandevuID { get; set; }
        public int PoliclinicID { get; set; }
        public int DoctorID { get; set; }
        public string CalismaSaati { get; set; }

        public Doctor? Doctor { get; set; }

        
    }
}
