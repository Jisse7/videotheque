using System.ComponentModel.DataAnnotations;

namespace videotheque.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage ="Nom requis")]
		public string Name { get; set; }


		[Required(ErrorMessage = "Email requis")]
		[EmailAddress]
		public string Email { get; set; }



		[Required(ErrorMessage = "Mot de passe requis")]
		[StringLength(40,MinimumLength =8, ErrorMessage ="8 caractères minimum")]
		[DataType(DataType.Password)]
		[Compare("ConfirmPassword", ErrorMessage ="Les mot des passes sont différents")]
		public string Password { get; set; }


		[Required(ErrorMessage = "Mot de passe de confirmation requis")]
		[DataType(DataType.Password)]

		[Display(Name = "Confirmer le nouveau Mot de passe")]
		public string ConfirmPassword { get; set; }
	}
}
