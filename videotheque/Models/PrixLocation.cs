namespace videotheque.Models
{
    public class PrixLocation
    {
        public int Id { get; set; }
        public int DureeHeures { get; set; }
        public decimal Prix { get; set; }

        // Clé étrangère
        public int FilmId { get; set; }
        public virtual Film Film { get; set; }
    }
}
