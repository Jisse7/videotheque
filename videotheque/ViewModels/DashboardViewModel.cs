using System.Collections.Generic;

namespace videotheque.ViewModels
{
    public class DashboardViewModel
    {
        // Statistiques générales
        public int TotalFilms { get; set; }
        public int TotalUsers { get; set; }
        public int TotalLocationsEnCours { get; set; }
        public decimal ChiffreAffairesMois { get; set; }

        // Prix moyens
        public decimal PrixMoyen24h { get; set; }
        public decimal PrixMoyen48h { get; set; }
        public decimal PrixMoyen72h { get; set; }
        public decimal PrixMoyen1Semaine { get; set; }

        // Listes pour le dashboard
        public List<LocationRecente> DernieresLocations { get; set; }
        public List<FilmPopulaire> TopFilmsLoues { get; set; }
        public List<FilmPopulaire> TopFilmsNotes { get; set; }
        public List<LocationRetard> LocationsEnRetard { get; set; }
        public List<Film> FilmsStockBas { get; set; }
        public List<UserBasic> TousLesUtilisateurs { get; set; }
        public List<UserNouveau> NouveauxUtilisateurs { get; set; }
        public List<DVDDisponible> DVDsDisponibles { get; set; }
    }

    // Remplace UserFidele par UserBasic
    public class UserBasic
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Adresse { get; set; }
        public DateTime DateInscription { get; set; }
    }

    // Les autres classes restent inchangées...
    public class LocationRecente
    {
        public string UserName { get; set; }
        public string FilmTitle { get; set; }
        public DateTime DateLocation { get; set; }
        public DateTime DateRetourPrevue { get; set; }
        public decimal MontantTotal { get; set; }
    }

    public class FilmPopulaire
    {
        public string Titre { get; set; }
        public int NombreLocations { get; set; }
        public double ScoreMoyen { get; set; }
        public int ExemplairesDisponibles { get; set; }
    }

    public class LocationRetard
    {
        public string UserName { get; set; }
        public string FilmTitle { get; set; }
        public DateTime DateRetourPrevue { get; set; }
        public int JoursRetard { get; set; }
    }

    public class UserNouveau
    {
        public string FullName { get; set; }
        public DateTime DateInscription { get; set; }
        public int NombreLocations { get; set; }
    }

    public class DVDDisponible
    {
        public string FilmTitle { get; set; }
        public string CodeBarre { get; set; }
        public int ExemplairesDisponibles { get; set; }
    }
}