using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tesst.Models;

namespace tesst.Controllers
{
    public class SeanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SeanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Seance
        public IActionResult Index()
        {
            var seances = _context.Seances.ToList();
            return View(seances); // Affiche la vue avec la liste des séances
        }

        // POST: Seance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seance seance)
        {
            if (ModelState.IsValid)
            {
                _context.Seances.Add(seance);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index)); // Redirige vers la liste des séances
            }
            return View(seance); // Retourne la vue en cas d'erreur de validation
        }
    }
}
