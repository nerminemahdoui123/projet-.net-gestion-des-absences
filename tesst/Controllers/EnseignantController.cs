using Microsoft.AspNetCore.Mvc;
using tesst.Models;
using System.Linq;

namespace tesst.Controllers
{
    public class EnseignantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnseignantController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Enseignant/Create
        public IActionResult Create()
        {
            // Récupérer les départements et grades pour les afficher dans les listes déroulantes
            ViewBag.DepartementList = _context.Departements.ToList();
            ViewBag.GradeList = _context.Grades.ToList();

            // Récupérer la liste des enseignants existants
            var enseignants = _context.Enseignants.ToList();

            // Passer les deux informations à la vue
            ViewData["EnseignantsList"] = enseignants;

            return View();
        }

        // POST: Enseignant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom, Prenom, DateRecrutement, Adresse, Mail, Tel, CodeDepartement, CodeGrade")] Enseignant enseignant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enseignant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create)); // Rediriger vers la même vue après l'ajout
            }

            // Si la validation échoue, renvoyer la vue avec les données nécessaires
            ViewBag.DepartementList = _context.Departements.ToList();
            ViewBag.GradeList = _context.Grades.ToList();
            ViewData["EnseignantsList"] = _context.Enseignants.ToList();
            return View(enseignant);
        }
    }
}