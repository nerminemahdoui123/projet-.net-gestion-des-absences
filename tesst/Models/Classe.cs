using System.ComponentModel.DataAnnotations;

namespace tesst.Models
{
    public class Classe
    {
        [Key]

        public int CodeClasse { get; set; } // Clé primaire
        public  string NomClasse { get; set; }

        public int CodeGroupe { get; set; } // Clé étrangère
        public  Groupe? Groupe { get; set; }

        public int CodeDepartement { get; set; } // Clé étrangère
        public  Departement? Departement { get; set; }

        public  ICollection<Etudiant>? Etudiants { get; set; }
        public  ICollection<FicheAbsence>? FichesAbsence { get; set; }
    }


}
