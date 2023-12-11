using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
	public class UserModel
	{
		public Guid Id { get; set; }
		public string? FullName { get; set; } // Burdaki soru işareti bu property nin null değer alabileceğini gösteriyor.
		public string Username { get; set; }
		public Boolean Locked { get; set; } = false;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public string? ProfileImageFileName { get; set; } = "Noimage3.png";
		public string Role { get; set; } = "user";
	}
}
