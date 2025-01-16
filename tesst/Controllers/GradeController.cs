using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tesst.Models;

namespace tesst.Controllers
{

    public class GradeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GradeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Grade/Index
        public IActionResult Index()
        {
            var grades = _context.Grades.ToList();
            return View(grades); // Affiche la vue avec la liste des grades
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Grade grade)
        {
            Console.WriteLine("Méthode Create appelée"); // Ajout pour vérifier l'appel
            if (ModelState.IsValid)
            {
                Console.WriteLine($"Grade soumis : {grade.NomGrade}");
                _context.Grades.Add(grade);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            Console.WriteLine("Le modèle est invalide");
            var grades = _context.Grades.ToList();
            return View("Index", grades);
        }


    }
}