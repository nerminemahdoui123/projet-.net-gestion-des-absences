using System.ComponentModel.DataAnnotations;

namespace tesst.Models
{
    public class Grade
    {
        [Key]
        public int CodeGrade { get; set; }

        [Required] // Remplacez 'required' par [Required] pour les versions de .NET antérieures à .NET 6.0
        public string NomGrade { get; set; }

        public ICollection<Enseignant>? Enseignants { get; set; }
    }


}
