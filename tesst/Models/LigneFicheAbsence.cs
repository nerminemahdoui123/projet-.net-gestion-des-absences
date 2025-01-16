using System.ComponentModel.DataAnnotations;

namespace tesst.Models
{
    public class LigneFicheAbsence
    {
        
        public int CodeFicheAbsence { get; set; } // Clé étrangère composite
        public  FicheAbsence? FicheAbsence { get; set; }

        public int CodeEtudiant { get; set; } // Clé étrangère composite
        public  Etudiant? Etudiant { get; set; }
    }

}
