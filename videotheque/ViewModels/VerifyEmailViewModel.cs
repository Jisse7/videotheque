using System.ComponentModel.DataAnnotations;

namespace videotheque.ViewModels
{
	public class VerifyEmailViewModel
	{

		[Required(ErrorMessage = "Email requis")]
		[EmailAddress]
		public string Email { get; set; }
	}
}
