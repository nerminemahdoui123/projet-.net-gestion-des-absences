using System.ComponentModel.DataAnnotations;

namespace tesst.Models
{
    public class FicheAbsenceSeance
    {
      

        public int CodeFicheAbsence { get; set; } // Clé étrangère composite
        public  FicheAbsence? FicheAbsence { get; set; }

        public int CodeSeance { get; set; } // Clé étrangère composite
        public  Seance? Seance { get; set; }
    }

}
