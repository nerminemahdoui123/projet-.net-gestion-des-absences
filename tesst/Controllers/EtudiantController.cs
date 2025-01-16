using Microsoft.AspNetCore.Mvc;
using tesst.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace tesst.Controllers
{
    public class EtudiantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EtudiantController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Affichage du formulaire et de la liste des étudiants
        public IActionResult Index()
        {
            // Récupérer tous les étudiants
            var etudiants = _context.Etudiants.Include(e => e.Classe).ToList();

            // Récupérer toutes les classes disponibles
            var classes = _context.Classes.ToList();

            // Passer les classes disponibles à la vue via ViewBag
            ViewBag.Classes = new SelectList(classes, "CodeClasse", "NomClasse");

            return View(etudiants);
        }

        // Ajouter un étudiant
        [HttpPost]
        public IActionResult AjouterEtudiant(Etudiant etudiant)
        {
            if (ModelState.IsValid)
            {
                _context.Etudiants.Add(etudiant);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Étudiant ajouté avec succès !";
            }
            else
            {
                TempData["ErrorMessage"] = "Une erreur s'est produite lors de l'ajout.";
            }
            return RedirectToAction("Index");
        }
        




    }
}
