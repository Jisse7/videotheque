using videotheque.Models;

namespace videotheque.ViewModels
{
    public class MovieGridViewModel
    {
        public List<Film> Films { get; set; }
        public List<Categorie> Categories { get; set; }
    }
}