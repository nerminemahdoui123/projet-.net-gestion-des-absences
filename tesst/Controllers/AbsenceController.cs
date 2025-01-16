using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tesst.Models;
using System.Linq;

namespace tesst.Controllers
{
    public class AbsenceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AbsenceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Absence/Create
        public IActionResult Create()
        {
            // Préparer les données pour remplir les listes déroulantes (ex: Matière, Enseignant, Classe, Etudiant)
            ViewBag.Matieres = _context.Matieres.ToList();
            ViewBag.Enseignants = _context.Enseignants.ToList();
            ViewBag.Classes = _context.Classes.ToList();
            ViewBag.Etudiants = _context.Etudiants.ToList();
            ViewBag.Seances = _context.Seances.ToList(); // Chargement des séances

            // Récupérer toutes les absences existantes et les passer à la vue
            ViewBag.Absences = _context.FichesAbsence
                                        .Include(f => f.Matiere)   // Inclure les informations de la Matière
                                        .Include(f => f.Enseignant) // Inclure les informations de l'Enseignant
                                        .Include(f => f.Classe)     // Inclure les informations de la Classe
                                        .Include(f => f.LignesFicheAbsence)
                                         // Inclure les lignes de chaque absence
                                         .Include(f => f.FichesAbsenceSeance)
                                        .ToList();

            return View();
        }

        // POST: Absence/Create
        // POST: Absence/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int codeMatiere, int codeEnseignant, int codeClasse, int codeEtudiant, DateTime dateJour, int codeSeance)
        {
            if (ModelState.IsValid)
            {
                // Créer une nouvelle FicheAbsence
                FicheAbsence ficheAbsence = new FicheAbsence
                {
                    DateJour = dateJour,
                    CodeMatiere = codeMatiere,
                    CodeEnseignant = codeEnseignant,
                    CodeClasse = codeClasse
                };

                // Ajouter la fiche d'absence à la base de données
                _context.FichesAbsence.Add(ficheAbsence);
                _context.SaveChanges(); // Save to get the CodeFicheAbsence

                // Créer la LigneFicheAbsence pour l'étudiant
                LigneFicheAbsence ligneFicheAbsence = new LigneFicheAbsence
                {
                    CodeFicheAbsence = ficheAbsence.CodeFicheAbsence,
                    CodeEtudiant = codeEtudiant
                };
                _context.LignesFicheAbsence.Add(ligneFicheAbsence);
                _context.SaveChanges();

                // Vérifier que la séance existe avant d'ajouter l'absence de séance
                var seance = _context.Seances.FirstOrDefault(s => s.CodeSeance == codeSeance);
                if (seance != null)
                {
                    // Créer la FicheAbsenceSeance pour l'absence de la séance
                    FicheAbsenceSeance ficheAbsenceSeance = new FicheAbsenceSeance
                    {
                        CodeFicheAbsence = ficheAbsence.CodeFicheAbsence,
                        CodeSeance = seance.CodeSeance // Associer la séance à l'absence
                    };
                    _context.FichesAbsenceSeance.Add(ficheAbsenceSeance);
                    _context.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("CodeSeance", "La séance spécifiée n'existe pas.");
                }

                return RedirectToAction(nameof(Create)); // Rediriger vers la page de création pour actualiser la liste
            }

            // Si le modèle n'est pas valide, retourner à la vue de création avec les erreurs de validation
            ViewBag.Matieres = _context.Matieres.ToList();
            ViewBag.Enseignants = _context.Enseignants.ToList();
            ViewBag.Classes = _context.Classes.ToList();
            ViewBag.Etudiants = _context.Etudiants.ToList();
            ViewBag.Seances = _context.Seances.ToList(); // Chargement des séances
            ViewBag.Absences = _context.FichesAbsence
                .Include(f => f.Matiere)
                .Include(f => f.Enseignant)
                .Include(f => f.Classe)
                .Include(f => f.LignesFicheAbsence)
                .Include(f => f.FichesAbsenceSeance)
                .ToList();

            return View();
        }
        [HttpGet("Absence/Search")]
        public IActionResult Search(int? codeClasse, string nomEtudiant, TimeSpan? heureDebut, TimeSpan? heureFin)
        {
            // Construire la requête de base avec les inclusions nécessaires
            var query = _context.FichesAbsence
                                .Include(f => f.Matiere)
                                .Include(f => f.Enseignant)
                                .Include(f => f.Classe)
                                .Include(f => f.LignesFicheAbsence)
                                .ThenInclude(l => l.Etudiant) // Inclure les détails des étudiants
                                .Include(f => f.FichesAbsenceSeance)
                                .ThenInclude(fs => fs.Seance)
                                .AsQueryable();

            // Appliquer les filtres dynamiques
            if (codeClasse.HasValue)
            {
                query = query.Where(f => f.CodeClasse == codeClasse.Value);
            }

            if (!string.IsNullOrEmpty(nomEtudiant))
            {
                query = query.Where(f => f.LignesFicheAbsence
                                               .Any(l => l.Etudiant.Nom.Contains(nomEtudiant)));
            }

            if (heureDebut.HasValue)
            {
                query = query.Where(f => f.FichesAbsenceSeance
                                               .Any(fs => fs.Seance.HeureDebut >= heureDebut.Value));
            }

            if (heureFin.HasValue)
            {
                query = query.Where(f => f.FichesAbsenceSeance
                                               .Any(fs => fs.Seance.HeureFin <= heureFin.Value));
            }

            // Exécuter la requête pour obtenir les résultats
            var resultats = query.ToList();

            // Grouper les absences par étudiant et par matière
            var absencesParEtudiantEtMatiere = resultats
                .SelectMany(f => f.LignesFicheAbsence.Select(l => new
                {
                    Etudiant = l.Etudiant,
                    Matiere = f.Matiere
                }))
                .GroupBy(x => new { x.Etudiant, x.Matiere })
                .Select(g => new
                {
                    Etudiant = g.Key.Etudiant.Nom,
                    Matiere = g.Key.Matiere.NomMatiere,
                    NombreAbsences = g.Count()
                })
                .OrderBy(g => g.Etudiant)
                .ThenBy(g => g.Matiere)
                .ToList();

            // Passer les données à la vue
            ViewBag.Absences = resultats;
            ViewBag.AbsencesParEtudiantEtMatiere = absencesParEtudiantEtMatiere;
            ViewBag.Matieres = _context.Matieres.ToList();
            ViewBag.Enseignants = _context.Enseignants.ToList();
            ViewBag.Classes = _context.Classes.ToList();
            ViewBag.Etudiants = _context.Etudiants.ToList();
            ViewBag.Seances = _context.Seances.ToList();

            return View("Search"); // Retourner la vue avec les données
        }


        // GET: Absence/Delete/{id}
        public IActionResult Delete(int id)
        {
            var ficheAbsence = _context.FichesAbsence
                                        .Include(f => f.LignesFicheAbsence)
                                        .Include(f => f.FichesAbsenceSeance)
                                        .FirstOrDefault(f => f.CodeFicheAbsence == id);

            if (ficheAbsence == null)
            {
                return NotFound();
            }

            // Supprimer les lignes de fiche d'absence et les fiches absence séance associées
            _context.LignesFicheAbsence.RemoveRange(ficheAbsence.LignesFicheAbsence);
            _context.FichesAbsenceSeance.RemoveRange(ficheAbsence.FichesAbsenceSeance);

            // Supprimer la fiche d'absence
            _context.FichesAbsence.Remove(ficheAbsence);
            _context.SaveChanges();

            return RedirectToAction(nameof(Create)); // Rediriger vers la page de création ou vers la liste des absences
        }


        // GET: Absence/Details
        // GET: Absence/Details
        // GET: Absence/Details
        public IActionResult Details(int codeEtudiant, int codeMatiere)
        {
            // Récupérer les absences pour cet étudiant et cette matière spécifiques
            var absencesDetails = _context.FichesAbsence
                                          .Include(f => f.Matiere)
                                          .Include(f => f.Enseignant)
                                          .Include(f => f.FichesAbsenceSeance)
                                          .ThenInclude(fs => fs.Seance)
                                          .Where(f => f.LignesFicheAbsence
                                                      .Any(l => l.CodeEtudiant == codeEtudiant) &&
                                                     f.CodeMatiere == codeMatiere)
                                          .ToList();

            // Vérifier s'il existe des absences
            if (absencesDetails == null || !absencesDetails.Any())
            {
                return NotFound();
            }

            // Passer les détails des absences à la vue
            ViewBag.AbsencesDetails = absencesDetails;

            return RedirectToAction(nameof(Details));// Retourner à la vue Details.cshtml
        }
        //ediiiiiit
        // GET: Absence/Edit/5
        public IActionResult Edit(int id)
        {
            // Récupérer la fiche d'absence à modifier en incluant les informations nécessaires
            var ficheAbsence = _context.FichesAbsence
                                        .Include(f => f.Matiere)
                                        .Include(f => f.Enseignant)
                                        .Include(f => f.Classe)
                                        .Include(f => f.LignesFicheAbsence)
                                        .Include(f => f.FichesAbsenceSeance)
                                        .FirstOrDefault(f => f.CodeFicheAbsence == id);

            if (ficheAbsence == null)
            {
                return NotFound();
            }

            // Passer les données nécessaires à la vue pour remplir les listes déroulantes
            ViewBag.Matieres = _context.Matieres.ToList();
            ViewBag.Enseignants = _context.Enseignants.ToList();
            ViewBag.Classes = _context.Classes.ToList();
            ViewBag.Etudiants = _context.Etudiants.ToList();
            ViewBag.Seances = _context.Seances.ToList();

            // Passer l'absence à la vue pour l'édition
            return View(ficheAbsence);
        }

        // POST: Absence/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, int codeMatiere, int codeEnseignant, int codeClasse, int codeEtudiant, DateTime dateJour, int codeSeance)
        {
            if (id != codeMatiere)  // Vérification pour s'assurer que l'id correspond
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Récupérer la fiche d'absence à modifier
                    var ficheAbsence = _context.FichesAbsence
                                                .FirstOrDefault(f => f.CodeFicheAbsence == id);
                    if (ficheAbsence == null)
                    {
                        return NotFound();
                    }

                    // Mettre à jour les propriétés de la fiche d'absence
                    ficheAbsence.DateJour = dateJour;
                    ficheAbsence.CodeMatiere = codeMatiere;
                    ficheAbsence.CodeEnseignant = codeEnseignant;
                    ficheAbsence.CodeClasse = codeClasse;

                    // Enregistrer les modifications de la fiche d'absence
                    _context.Update(ficheAbsence);
                    _context.SaveChanges();

                    // Mettre à jour les lignes de la fiche d'absence (si nécessaire)
                    var ligneFicheAbsence = _context.LignesFicheAbsence
                                                    .FirstOrDefault(l => l.CodeFicheAbsence == id && l.CodeEtudiant == codeEtudiant);
                    if (ligneFicheAbsence != null)
                    {
                        // Vous pouvez mettre à jour d'autres informations liées à la ligne si nécessaire
                        _context.Update(ligneFicheAbsence);
                    }

                    // Mettre à jour la séance si nécessaire
                    var ficheAbsenceSeance = _context.FichesAbsenceSeance
                                                      .FirstOrDefault(f => f.CodeFicheAbsence == id && f.CodeSeance == codeSeance);
                    if (ficheAbsenceSeance != null)
                    {
                        ficheAbsenceSeance.CodeSeance = codeSeance;  // Par exemple, changer la séance
                        _context.Update(ficheAbsenceSeance);
                    }

                    // Sauvegarder les changements
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.FichesAbsence.Any(e => e.CodeFicheAbsence == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Create)); // Rediriger vers la page de création après l'édition
            }

            // Si le modèle est invalide, retourner à la vue d'édition avec les erreurs
            ViewBag.Matieres = _context.Matieres.ToList();
            ViewBag.Enseignants = _context.Enseignants.ToList();
            ViewBag.Classes = _context.Classes.ToList();
            ViewBag.Etudiants = _context.Etudiants.ToList();
            ViewBag.Seances = _context.Seances.ToList();

            return View();
        }








    }
}

