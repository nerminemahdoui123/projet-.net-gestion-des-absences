using Microsoft.EntityFrameworkCore;
using tesst;
using tesst.Models;

var builder = WebApplication.CreateBuilder(args);

// Ajout des services nécessaires à l'application
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor(); // Ajoutez cette ligne dans votre configuration des services

// Configuration de la base de données avec Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Ajout des services pour les sessions
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Durée de session de 30 minutes
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configuration du pipeline de requêtes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HSTS pour renforcer la sécurité en production
}


app.UseHttpsRedirection();
app.UseStaticFiles(); // Activer les fichiers statiques (ex. CSS, JS)

app.UseRouting();

// Ajout des sessions et de l'autorisation
app.UseSession();
app.UseAuthorization();

// Configuration des routes de l'application
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}" // Par défaut, démarre sur la page Login
);

app.Run();
