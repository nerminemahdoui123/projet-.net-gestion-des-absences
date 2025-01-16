using Microsoft.AspNetCore.Mvc;
using tesst.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace tesst.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Injecter le contexte de la base de données
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Afficher la page de création d'un utilisateur
        public IActionResult CreateUser()
        {
            // Récupérer tous les utilisateurs depuis la base de données
            var users = _context.Users.ToList();
            return View(users);  // Passer la liste des utilisateurs à la vue
        }

        // Gérer la soumission du formulaire pour créer un utilisateur
        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                // Hash du mot de passe avant de l'ajouter à la base de données
                user.Password = HashPassword(user.Password);

                // Ajouter l'utilisateur à la base de données
                _context.Users.Add(user);
                _context.SaveChanges();  // Sauvegarder les modifications dans la base de données

                // Récupérer à nouveau la liste des utilisateurs pour la passer à la vue
                var users = _context.Users.ToList();
                return View(users);  // Retourner la vue avec la liste mise à jour
            }

            // Si le modèle est invalide, revenir au formulaire de création
            return View(user);
        }

        // Fonction pour hasher le mot de passe
        private string HashPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: System.Text.Encoding.UTF8.GetBytes("some_salt_here"),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        // Liste des utilisateurs (pour les afficher dans la vue)
        public IActionResult Index()
        {
            var users = _context.Users.ToList();  // Récupère tous les utilisateurs depuis la base de données
            return View(users);  // Passe la liste d'utilisateurs à la vue
        }


        // Modifier un utilisateur
        public IActionResult EditUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // Supprimer un utilisateur
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

       

    }

}
