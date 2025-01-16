using System.ComponentModel.DataAnnotations;

namespace tesst.Models
{
    public class FicheAbsence
    {
        internal readonly object CodeEtudiant;

        public int CodeFicheAbsence { get; set; } // Clé primaire
        public DateTime DateJour { get; set; }

        public int CodeMatiere { get; set; } // Clé étrangère
        public  Matiere? Matiere { get; set; }

        public int CodeEnseignant { get; set; } // Clé étrangère
        public  Enseignant? Enseignant { get; set; }

        public int CodeClasse { get; set; } // Clé étrangère
        public  Classe? Classe { get; set; }

        public  ICollection<LigneFicheAbsence>? LignesFicheAbsence { get; set; }
        public  ICollection<FicheAbsenceSeance>? FichesAbsenceSeance { get; set; }
        
    }

}
