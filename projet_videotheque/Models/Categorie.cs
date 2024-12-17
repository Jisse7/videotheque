using System.ComponentModel.DataAnnotations;

namespace videotheque.Models
{
    public class Categorie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de la catégorie est requis")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "La description de la catégorie est requise")]
        public string Description { get; set; }

        // Propriété de navigation
        public virtual ICollection<Film>? Films { get; set; }

        public Categorie()
        {
            Films = new HashSet<Film>();
        }
    }
}