using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Entities
{
	[Table("Users")]
	public class User
	{
		[Key]
		public Guid Id { get; set; }


		[StringLength(50)]
		public string? FullName { get; set; } // Burdaki soru işareti bu property nin null değer alabileceğini gösteriyor.
		[Required]
		[StringLength(30)]
		public string Username { get; set; }
		
		[Required]
		[StringLength(100)]
		public string Password { get; set; }
		public Boolean Locked { get; set; } = false;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
	}
}
