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

        // Injecter le contexte de la base de donn�es
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

        // V�rifie les informations de connexion
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // V�rification manuelle de l'administrateur
            if (username == "admin" && password == "admin")
            {
                HttpContext.Session.SetString("User", username);
                return RedirectToAction("Index");
            }

            // V�rification des informations dans la base de donn�es pour les autres utilisateurs
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user != null && VerifyPassword(password, user.Password))  // Comparaison avec le mot de passe hach�
            {
                HttpContext.Session.SetString("User", username);
                return RedirectToAction("IndexUser");
            }

            ViewBag.Error = "Nom d'utilisateur ou mot de passe incorrect.";
            return View();
        }

        // Fonction pour v�rifier le mot de passe
        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            // Vous pouvez utiliser la m�me m�thode que pour le hachage du mot de passe
            var salt = System.Text.Encoding.UTF8.GetBytes("some_salt_here");  // Assurez-vous d'utiliser le m�me salt utilis� lors de l'enregistrement
            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return storedPasswordHash == hashedPassword;
        }

        // D�connexion
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Index()
        {
            // V�rifiez si l'utilisateur est connect�
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        public IActionResult IndexUser()
        {
            // V�rifiez si l'utilisateur est connect�
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            {
                return RedirectToAction("Login");
            }

            return View(); // Retourne la vue IndexUser
        }

    }
}
