using System.ComponentModel.DataAnnotations;

namespace videotheque.ViewModels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Le nom est requis")]
        [Display(Name = "Nom complet")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; }

        [Display(Name = "Adresse")]
        public string? Address { get; set; }

        [Display(Name = "Numéro de téléphone")]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nouveau mot de passe")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        [Compare("NewPassword", ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string? ConfirmPassword { get; set; }
    }
}