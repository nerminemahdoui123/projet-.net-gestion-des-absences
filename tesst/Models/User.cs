using System.ComponentModel.DataAnnotations;

namespace tesst.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nom d'utilisateur")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // "enseignant" ou "utilisateur"
    }
}
