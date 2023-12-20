using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Entities
{
    public class Doctor
    {
        public int DoctorID { get; set; }
        [Required]
        public string DoctorName { get; set;}
        public int PoliclinicID { get; set; }//Her bir doktora ait polikinliği görmek için.
        public string WorkingHours { get; set; }    
        public Policlinic? Policlinic { get; set; }
    }
}
