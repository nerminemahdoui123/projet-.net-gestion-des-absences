using Microsoft.AspNetCore.Mvc;
using tesst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text.RegularExpressions;

namespace tesst.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Injecter le contexte de la base de données
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Affiche le formulaire de connexion
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Vérifie les informations de connexion
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Vérification manuelle de l'administrateur
            if (username == "admin" && password == "admin")
            {
                HttpContext.Session.SetString("User", username);
                return RedirectToAction("Index");
            }

            // Vérification des informations dans la base de données pour les autres utilisateurs
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user != null && VerifyPassword(password, user.Password))  // Comparaison avec le mot de passe haché
            {
                HttpContext.Session.SetString("User", username);
                return RedirectToAction("IndexUser");
            }

            ViewBag.Error = "Nom d'utilisateur ou mot de passe incorrect.";
            return View();
        }

        // Fonction pour vérifier le mot de passe
        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            // Vous pouvez utiliser la même méthode que pour le hachage du mot de passe
            var salt = System.Text.Encoding.UTF8.GetBytes("some_salt_here");  // Assurez-vous d'utiliser le même salt utilisé lors de l'enregistrement
            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return storedPasswordHash == hashedPassword;
        }

        // Déconnexion
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Index()
        {
            // Vérifiez si l'utilisateur est connecté
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        public IActionResult IndexUser()
        {
            // Vérifiez si l'utilisateur est connecté
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            {
                return RedirectToAction("Login");
            }

            return View(); // Retourne la vue IndexUser
        }

    }
}
