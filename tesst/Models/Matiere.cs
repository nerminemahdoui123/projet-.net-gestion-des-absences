using System.ComponentModel.DataAnnotations;

namespace tesst.Models
{
    public class Matiere
    {
        
        public int CodeMatiere { get; set; } // Clé primaire
        public  string? NomMatiere { get; set; }
        public int NbreHeureCoursParSemaine { get; set; }
        public int NbreHeureTDParSemaine { get; set; }
        public int NbreHeureTPParSemaine { get; set; }
    }


}
