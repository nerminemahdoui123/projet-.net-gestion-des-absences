using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using tesst.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;



namespace tesst.Controllers
{
    public class ClasseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClasseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Classe/Create
        public IActionResult Create()
        {
            // Assurez-vous que les listes sont bien initialisées
            ViewBag.GroupeList = _context.Groupes.ToList();
            ViewBag.DepartementList = _context.Departements.ToList();

            // Charger toutes les classes existantes pour l'affichage
            var classes = _context.Classes.Include(c => c.Groupe).Include(c => c.Departement).ToList();
            ViewBag.ClassesList = classes;

            return View();
        }

        // POST: Classe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomClasse, CodeGroupe, CodeDepartement")] Classe classe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classe);
                await _context.SaveChangesAsync();

                // Affichage de la classe créée dans la même vue
                ViewBag.Message = "Classe ajoutée avec succès!";
                ViewBag.CreatedClasse = classe; // Passer la classe créée à la vue

                // Charger toutes les classes existantes pour l'affichage après ajout
                var classes = await _context.Classes.Include(c => c.Groupe).Include(c => c.Departement).ToListAsync();
                ViewBag.ClassesList = classes;

                // Charger les listes de groupes et départements
                ViewBag.GroupeList = _context.Groupes.ToList();
                ViewBag.DepartementList = _context.Departements.ToList();

                return View(); // Retourner à la même vue avec les données mises à jour
            }

            // Si la validation échoue, renvoyer la vue avec les mêmes données
            ViewBag.GroupeList = _context.Groupes.ToList();
            ViewBag.DepartementList = _context.Departements.ToList();
            return View(classe);
        }
        // POST: Classe/DeleteClasse/5 (Méthode AJAX)
        [HttpPost]
        public async Task<IActionResult> DeleteClasse(int codeClasse)
        {
            var classe = await _context.Classes.FindAsync(codeClasse);
            if (classe != null)
            {
                _context.Classes.Remove(classe);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Classe supprimée avec succès!" });
            }
            return Json(new { success = false, message = "Classe non trouvée!" });
        }




    }
}