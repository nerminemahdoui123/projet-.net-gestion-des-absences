using Microsoft.EntityFrameworkCore;
using tesst.Models;

namespace tesst
{
    public class ApplicationDbContext : DbContext
    {
        internal readonly object FicheAbsences;

        public DbSet<Departement> Departements { get; set; }
        public DbSet<Groupe> Groupes { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Classe> Classes { get; set; }
        public DbSet<Matiere> Matieres { get; set; }
        public DbSet<Seance> Seances { get; set; }
        public DbSet<Enseignant> Enseignants { get; set; }
        public DbSet<Etudiant> Etudiants { get; set; }
        public DbSet<FicheAbsence> FichesAbsence { get; set; }
        public DbSet<FicheAbsenceSeance> FichesAbsenceSeance { get; set; }
        public DbSet<LigneFicheAbsence> LignesFicheAbsence { get; set; }
        public DbSet<User> Users { get; set; }


        // Constructeur avec DbContextOptions
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Clé Composite pour FicheAbsenceSeance
            modelBuilder.Entity<FicheAbsenceSeance>()
                .HasKey(fas => new { fas.CodeFicheAbsence, fas.CodeSeance });

            modelBuilder.Entity<FicheAbsenceSeance>()
                .HasOne(fas => fas.FicheAbsence)
                .WithMany(fa => fa.FichesAbsenceSeance)
                .HasForeignKey(fas => fas.CodeFicheAbsence)
                .OnDelete(DeleteBehavior.NoAction);  // Ajout de OnDelete NoAction

            modelBuilder.Entity<FicheAbsenceSeance>()
                .HasOne(fas => fas.Seance)
                .WithMany()
                .HasForeignKey(fas => fas.CodeSeance)
                .OnDelete(DeleteBehavior.NoAction);  // Ajout de OnDelete NoAction

            // Clé Composite pour LigneFicheAbsence
            modelBuilder.Entity<LigneFicheAbsence>()
                .HasKey(lfa => new { lfa.CodeFicheAbsence, lfa.CodeEtudiant });

            modelBuilder.Entity<LigneFicheAbsence>()
                .HasOne(lfa => lfa.FicheAbsence)
                .WithMany(fa => fa.LignesFicheAbsence)
                .HasForeignKey(lfa => lfa.CodeFicheAbsence)
                .OnDelete(DeleteBehavior.NoAction);  // Ajout de OnDelete NoAction

            modelBuilder.Entity<LigneFicheAbsence>()
                .HasOne(lfa => lfa.Etudiant)
                .WithMany()
                .HasForeignKey(lfa => lfa.CodeEtudiant)
                .OnDelete(DeleteBehavior.NoAction);  // Ajout de OnDelete NoAction

            // Relation : Enseignant -> Departement (1-N)
            modelBuilder.Entity<Enseignant>()
                .HasOne(e => e.Departement)
                .WithMany(d => d.Enseignants)
                .HasForeignKey(e => e.CodeDepartement)
                .OnDelete(DeleteBehavior.NoAction);  // Ajout de OnDelete NoAction

            // Relation : Enseignant -> Grade (1-N)
            modelBuilder.Entity<Enseignant>()
                .HasOne(e => e.Grade)
                .WithMany(g => g.Enseignants)
                .HasForeignKey(e => e.CodeGrade)
                .OnDelete(DeleteBehavior.NoAction);  // Ajout de OnDelete NoAction

            // Relation : Classe -> Groupe (1-N)
            modelBuilder.Entity<Classe>()
                .HasOne(c => c.Groupe)
                .WithMany(g => g.Classes)
                .HasForeignKey(c => c.CodeGroupe)
                .OnDelete(DeleteBehavior.NoAction);  // Ajout de OnDelete NoAction

            // Relation : Classe -> Departement (1-N)
            modelBuilder.Entity<Classe>()
                .HasOne(c => c.Departement)
                .WithMany(d => d.Classes)
                .HasForeignKey(c => c.CodeDepartement)
                .OnDelete(DeleteBehavior.NoAction);  // Ajout de OnDelete NoAction

            // Relation : Etudiant -> Classe (1-N)
            modelBuilder.Entity<Etudiant>()
                .HasOne(e => e.Classe)
                .WithMany(c => c.Etudiants)
                .HasForeignKey(e => e.CodeClasse)
                .OnDelete(DeleteBehavior.NoAction);  // Ajout de OnDelete NoAction

            // Relation : FicheAbsence -> Matiere (1-N)
            modelBuilder.Entity<FicheAbsence>()
                .HasOne(fa => fa.Matiere)
                .WithMany()
                .HasForeignKey(fa => fa.CodeMatiere)
                .OnDelete(DeleteBehavior.NoAction);  // Ajout de OnDelete NoAction

            // Relation : FicheAbsence -> Enseignant (1-N)
            modelBuilder.Entity<FicheAbsence>()
                .HasOne(fa => fa.Enseignant)
                .WithMany()
                .HasForeignKey(fa => fa.CodeEnseignant)
                .OnDelete(DeleteBehavior.NoAction);  // Ajout de OnDelete NoAction

            // Relation : FicheAbsence -> Classe (1-N)
            modelBuilder.Entity<FicheAbsence>()
                .HasOne(fa => fa.Classe)
                .WithMany(c => c.FichesAbsence)
                .HasForeignKey(fa => fa.CodeClasse)
                .OnDelete(DeleteBehavior.NoAction);  // Ajout de OnDelete NoAction

            // Configuration de la table Seance (si elle est indépendante)
            modelBuilder.Entity<Seance>()
                .HasKey(s => s.CodeSeance);

            // Configuration de la table Matiere
            modelBuilder.Entity<Matiere>()
                .HasKey(m => m.CodeMatiere);

            // Configuration de la table Groupe
            modelBuilder.Entity<Groupe>()
                .HasKey(g => g.CodeGroupe);

            // Configuration de la table Grade
            modelBuilder.Entity<Grade>()
                .HasKey(g => g.CodeGrade);

            // Configuration de la table Departement
            modelBuilder.Entity<Departement>()
                .HasKey(d => d.CodeDepartement);

            // Configuration de la table Classe
            modelBuilder.Entity<Classe>()
                .HasKey(c => c.CodeClasse);

            // Configuration de la table Enseignant
            modelBuilder.Entity<Enseignant>()
                .HasKey(e => e.CodeEnseignant);

            // Configuration de la table Etudiant
            modelBuilder.Entity<Etudiant>()
                .HasKey(e => e.CodeEtudiant);

            // Configuration de la table FicheAbsence
            modelBuilder.Entity<FicheAbsence>()
                .HasKey(fa => fa.CodeFicheAbsence);
        }
    }
}
