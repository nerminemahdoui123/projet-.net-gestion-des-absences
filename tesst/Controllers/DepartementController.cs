using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tesst.Models;

namespace tesst.Controllers
{
    public class DepartementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Departement
        public IActionResult Index()
        {
            var departements = _context.Departements.ToList();
            return View(departements);
        }

        // GET: Departement/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomDepartement")] Departement departement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(departement);
        }
    }
}