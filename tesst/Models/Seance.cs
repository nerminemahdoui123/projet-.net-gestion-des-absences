using System.ComponentModel.DataAnnotations;

namespace tesst.Models
{
    public class Seance
    {
        
        public int CodeSeance { get; set; } // Clé primaire
        public  string? NomSeance { get; set; }
        public TimeSpan HeureDebut { get; set; }
        public TimeSpan HeureFin { get; set; }
    }


}
