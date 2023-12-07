using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
	public class TurLoginViewModel
	{
		//[Display(Name ="Kullanıcı Adı", Prompt="Johndoe")] Burdaki prompt placeholder yerine geçiyor.
		[Required(ErrorMessage = "Kullanıcı adı zorunlu!!!")]
		[StringLength(30, ErrorMessage = "Kullanıcı adı en az 30 karakter olmalı!!!")]
		public string Username { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Şifre zorunlu")]
		[MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalı!!!")]
		[MaxLength(16, ErrorMessage = "Şifre en az 16 karakter olmalı!!!")]
		public string Password { get; set; }

	}
}
