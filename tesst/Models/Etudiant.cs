using System.ComponentModel.DataAnnotations;

namespace tesst.Models
{
    public class Etudiant
    {
        [Key]

        public int CodeEtudiant { get; set; } // Clé primaire
        [StringLength(100)]
        public required string Nom { get; set; }
        public required string Prenom { get; set; }
        public DateTime DateNaissance { get; set; }
        public required string Adresse { get; set; }
        public required string Mail { get; set; }
        public required string Tel { get; set; }
        public int NumInscription { get; set; }

        public int? CodeClasse { get; set; } // Nullable Foreign Key
        public Classe? Classe { get; set; }  // Nullable Navigation Property

    }


}
