using Microsoft.AspNetCore.Identity;

namespace videotheque.Models
{
  
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
        public string? Adresse { get; set; }  // ? → rend l'adresse nullable
        public DateTime DateInscription { get; set; } = DateTime.UtcNow;  // Valeur par défaut
    

    public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
