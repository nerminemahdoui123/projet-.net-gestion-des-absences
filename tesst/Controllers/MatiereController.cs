using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tesst.Models;

namespace tesst.Controllers
{
    namespace tesst.Controllers
    {
        public class MatiereController : Controller
        {
            private readonly ApplicationDbContext _context;

            public MatiereController(ApplicationDbContext context)
            {
                _context = context;
            }

            // GET: Matiere
            public IActionResult Index()
            {
                // Get the list of all Matieres from the database
                var matieres = _context.Matieres.ToList();
                return View(matieres); // Return the view with the list of Matieres
            }

            // POST: Matiere/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create(Matiere matiere)
            {
                if (ModelState.IsValid)
                {
                    // Add the new Matiere to the database
                    _context.Matieres.Add(matiere);
                    _context.SaveChanges();

                    // Redirect to Index to refresh the list of Matieres
                    return RedirectToAction(nameof(Index));
                }
                return View(matiere); // Return the same view in case of error
            }
        }
    }
}