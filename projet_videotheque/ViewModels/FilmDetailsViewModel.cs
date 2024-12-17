namespace videotheque.ViewModels
{
    public class FilmDetailsViewModel
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Realisateur { get; set; }
        public int AnneeSortie { get; set; }
        public string Synopsis { get; set; }
        public string ImageUrl { get; set; }
        public string TrailerUrl { get; set; }
        public double ScoreMoyen { get; set; }
        public int DureeMinutes { get; set; }
        public bool EstDisponible { get; set; }
        public int NombreExemplairesDisponibles { get; set; }
        public decimal Prix24h { get; set; }
        public decimal Prix48h { get; set; }
        public decimal Prix72h { get; set; }
        public decimal Prix1Semaine { get; set; }
        public string CategorieNom { get; set; }
    }
}
