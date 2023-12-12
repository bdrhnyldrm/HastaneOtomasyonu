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

	public class CreateUserModel
	{
		[Required(ErrorMessage = "Username is required!!!")]
		[StringLength(30, ErrorMessage = "Username can be max 30 characters.")]
		public string Username { get; set; }

		[Required]
		[StringLength(50)]
		public string FullName { get; set; }

		public bool Locked { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Password is required!!!")]
		[MinLength(6, ErrorMessage = "Password is can be min 6 characters.")]
		[MaxLength(16, ErrorMessage = "Password is can be max 16 characters.")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Re-Password is required!!!")]
		[MinLength(6, ErrorMessage = "Re-Password is can be min 6 characters.")]
		[MaxLength(16, ErrorMessage = "Re-Password is can be max 16 characters.")]
		[Compare(nameof(Password), ErrorMessage = "Don't matched the passwords")]
		public string RePassword { get; set; }

		[Required]
		[StringLength(50)]
		public string Role { get; set; } = "user";
	}

	public class EditUserModel
	{
		[Required(ErrorMessage = "Username is required!!!")]
		[StringLength(30, ErrorMessage = "Username can be max 30 characters.")]
		public string Username { get; set; }

		[Required]
		[StringLength(50)]
		public string FullName { get; set; }

		public bool Locked { get; set; }

		

		[Required]
		[StringLength(50)]
		public string Role { get; set; } = "user";
	}
}
