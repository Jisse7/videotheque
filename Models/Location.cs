namespace videotheque.Models
{
    public class Location
    {
        public int Id { get; set; }
        public DateTime DateLocation { get; set; }
        public DateTime DateRetourPrevue { get; set; }
        public DateTime? DateRetourEffective { get; set; }
        public decimal MontantTotal { get; set; }
        public string Statut { get; set; } // En cours, Terminée, En retard

        // Clés étrangères
        public string UserId { get; set; }
        public virtual Users User { get; set; }

        // Propriétés de navigation
        public virtual Paiement Paiement { get; set; }
        public virtual ICollection<LocationDetail> LocationDetails { get; set; }
    }
}