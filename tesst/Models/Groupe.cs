using System.ComponentModel.DataAnnotations;

namespace tesst.Models
{
    public class Groupe
    {
        
        public int CodeGroupe { get; set; } // Clé primaire
        public required string NomGroupe { get; set; }

        public  ICollection<Classe>? Classes { get; set; }
    }

}
