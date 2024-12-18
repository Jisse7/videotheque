namespace videotheque.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string CartId { get; set; }
        public DateTime DateAjout { get; set; }
        public int DureeLocation { get; set; }
        public decimal PrixLocation { get; set; }
        public int FilmId { get; set; }
        public string UserId { get; set; }
        public virtual Film Film { get; set; }
        public virtual Users User { get; set; }
    }
}