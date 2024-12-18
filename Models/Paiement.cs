namespace videotheque.Models
{
    public class Paiement
    {
        public int Id { get; set; }
        public decimal Montant { get; set; }
        public DateTime DatePaiement { get; set; }
        public string MethodePaiement { get; set; }
        public string StatutPaiement { get; set; }

        // Clé étrangère
        public int LocationId { get; set; }

        // Propriété de navigation
        public virtual Location Location { get; set; }
    }
}