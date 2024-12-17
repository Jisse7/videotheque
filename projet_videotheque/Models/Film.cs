using videotheque.Models;
//table pour film
public class Film
{
    public int Id { get; set; }
    public string Titre { get; set; }
    public string Realisateur { get; set; }
    public int AnneeSortie { get; set; }
    public string Synopsis { get; set; }
    public string ImageUrl { get; set; }
    public string TrailerUrl { get; set; }
    public int NombreExemplaires { get; set; }
    public int ExemplairesDisponibles { get; set; }
    public double ScoreMoyen { get; set; }
    public int DureeMinutes { get; set; }

   // champs pour les prix
    public decimal Prix24h { get; set; }
    public decimal Prix48h { get; set; }
    public decimal Prix72h { get; set; }
    public decimal Prix1Semaine { get; set; }

    public int CategorieId { get; set; }

    // Propriétés de navigation
    public virtual Categorie Categorie { get; set; }
    public virtual ICollection<Location> Locations { get; set; }
    public virtual ICollection<Evaluation> Evaluations { get; set; }
    public virtual ICollection<CartItem> CartItems { get; set; }
}