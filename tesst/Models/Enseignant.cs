using System.ComponentModel.DataAnnotations;

namespace tesst.Models
{
    public class Enseignant
    {
        
        public int CodeEnseignant { get; set; } // Clé primaire
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime DateRecrutement { get; set; }
        public string Adresse { get; set; }
        public string Mail { get; set; }
        public string Tel { get; set; }

        public int CodeDepartement { get; set; } // Clé étrangère
        public Departement? Departement { get; set; }

        public int CodeGrade { get; set; } // Clé étrangère
        public Grade? Grade { get; set; }
    }


}
