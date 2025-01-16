using System.ComponentModel.DataAnnotations;

namespace tesst.Models
{
    public class Departement
    {
        
        public int CodeDepartement { get; set; } // Clé primaire
        public string NomDepartement { get; set; }

        public ICollection<Enseignant>? Enseignants { get; set; }
        public ICollection<Classe>? Classes { get; set; }
    }

}
