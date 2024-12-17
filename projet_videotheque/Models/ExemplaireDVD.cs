namespace videotheque.Models
{
    public class ExemplaireDVD
    {
        public int Id { get; set; }

        // Code-barres unique pour chaque DVD physique
        public string CodeBarre { get; set; }

        // Indique si le DVD est actuellement dans le magasin
        public bool EstDansStock { get; set; }

        // Clé étrangère vers le film
        public int FilmId { get; set; }

        // Propriété de navigation vers le film
        public virtual Film Film { get; set; }

        // Propriété de navigation optionnelle vers la location en cours
        public virtual LocationDetail? LocationDetail { get; set; }
    }
}