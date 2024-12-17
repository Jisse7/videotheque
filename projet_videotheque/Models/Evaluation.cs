namespace videotheque.Models
{
    public class Evaluation
    {
        public int Id { get; set; }
        public int Score { get; set; } // Score de 1 à 5
        public DateTime DateEvaluation { get; set; }

        // Clés étrangères
        public string UserId { get; set; }
        public int FilmId { get; set; }

        // Propriétés de navigation
        public virtual Users User { get; set; }
        public virtual Film Film { get; set; }
    }
}
