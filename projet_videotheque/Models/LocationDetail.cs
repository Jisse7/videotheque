using videotheque.Models;



public class LocationDetail
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public int FilmId { get; set; }
    public decimal PrixUnitaire { get; set; }
    public int DureeLocation { get; set; }

    // Ajout de la référence à l'exemplaire DVD
    public int ExemplaireDVDId { get; set; }

    public virtual Location Location { get; set; }
    public virtual Film Film { get; set; }
    public virtual ExemplaireDVD ExemplaireDVD { get; set; }
}