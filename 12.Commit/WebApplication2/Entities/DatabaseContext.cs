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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Doctor)
                .WithMany(d => d.Randevular)
                .HasForeignKey(r => r.DoctorID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Policlinic)
                .WithMany(p => p.Randevular)
                .HasForeignKey(r => r.PoliclinicID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
