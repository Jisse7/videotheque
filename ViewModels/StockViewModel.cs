using System.ComponentModel.DataAnnotations;

namespace videotheque.ViewModels
{
    public class StockViewModel
    {
        [Required(ErrorMessage = "Le titre est requis")]
        public string Titre { get; set; }

        [Required(ErrorMessage = "Le réalisateur est requis")]
        public string Realisateur { get; set; }

        [Required(ErrorMessage = "L'année de sortie est requise")]
        [Range(1900, 2100, ErrorMessage = "L'année doit être comprise entre 1900 et 2100")]
        public int AnneeSortie { get; set; }

        [Required(ErrorMessage = "Le synopsis est requis")]
        public string Synopsis { get; set; }

        public string ImageUrl { get; set; }
        public string TrailerUrl { get; set; }
        public int NombreExemplaires { get; set; }
        public int ExemplairesDisponibles { get; set; }
        public double ScoreMoyen { get; set; }

        [Required(ErrorMessage = "La durée est requise")]
        [Range(1, int.MaxValue, ErrorMessage = "La durée doit être supérieure à 0")]
        public int DureeMinutes { get; set; }

        [Required(ErrorMessage = "Le prix 24h est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
        public decimal Prix24h { get; set; }

        [Required(ErrorMessage = "Le prix 48h est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
        public decimal Prix48h { get; set; }

        [Required(ErrorMessage = "Le prix 72h est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
        public decimal Prix72h { get; set; }

        [Required(ErrorMessage = "Le prix 1 semaine est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
        public decimal Prix1Semaine { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "La catégorie est requise")]
        public int CategorieId { get; set; }

        public string? CategorieName { get; set; }  // on l'a marqué comme nullable ??
    }

    public class CreateFilmViewModel
{
    public string Titre { get; set; }
    public string Realisateur { get; set; }
    public int AnneeSortie { get; set; }
    public string Synopsis { get; set; }
    public string ImageUrl { get; set; }
    public string TrailerUrl { get; set; }
    public int NombreExemplaires { get; set; }
    public int DureeMinutes { get; set; }
    public decimal Prix24h { get; set; }
    public decimal Prix48h { get; set; }
    public decimal Prix72h { get; set; }
    public decimal Prix1Semaine { get; set; }
    public int CategorieId { get; set; }
}
}