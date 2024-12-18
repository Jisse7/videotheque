using System.ComponentModel.DataAnnotations;

namespace videotheque.ViewModels
{
	public class ChangePasswordViewModel
	{

		[Required(ErrorMessage = "Email requis")]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = "Mot de passe requis")]
		[StringLength(40, MinimumLength = 8, ErrorMessage = "8 caractères minimum")]
		[DataType(DataType.Password)]
		[Display(Name = "Nouveau Mot de passe")]
		[Compare("ConfirmNewPassword", ErrorMessage = "Les mot des passes sotn différents")]
		public string NewPassword { get; set; }


		[Required(ErrorMessage = "Mot de passe de confirmation requis")]
		[DataType(DataType.Password)]
		[Display(Name ="Confirmer le nouveau Mot de passe")]
		public string ConfirmNewPassword { get; set; }

	}
}
