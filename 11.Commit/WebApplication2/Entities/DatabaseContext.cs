using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Entities
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions options) : base(options)
		{

		}

		public DbSet<User> Users { get; set; }//User sınıfı veri tabanıyla eşleştiğinde user tablosu oluşturacak ve isim olarak Users koyacak.
		public DbSet<Policlinic> Policlinics { get; set; }
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Randevu> Randevular {  get; set; }
	}
}
