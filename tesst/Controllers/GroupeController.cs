using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tesst;
using tesst.Models;

public class GroupeController : Controller
{
    private readonly ApplicationDbContext _context;

    public GroupeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Groupe
    public IActionResult Index()
    {
        var groupes = _context.Groupes.ToList();
        return View(groupes);
    }

    // GET: Groupe/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Groupe/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NomGroupe")] Groupe groupe)
    {
        if (ModelState.IsValid)
        {
            _context.Add(groupe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(groupe);
    }
}

