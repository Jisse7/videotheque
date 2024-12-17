using System.ComponentModel.DataAnnotations;

namespace videotheque.ViewModels
{
	public class LoginViewModel
	{

		[Required(ErrorMessage = "Email requis")]
		[EmailAddress]
		public string Email { get; set; }


		[Required(ErrorMessage = "Mot de passe requis")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name ="Se rappeler de moi?")]

		public bool RememberMe { get; set; }
	}
}
