using Microsoft.EntityFrameworkCore;
using WebApplication24_02.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container for Razor Pages and MVC controllers
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/"); // Bloquea TODO el sitio
    options.Conventions.AllowAnonymousToPage("/Account/Login"); // Deja entrar al Login sin clave
});
builder.Services.AddControllersWithViews();

// 1. CONFIGURACIÓN DEL LOGIN (COOKIES)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Página a la que manda si no estás logueado
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// 1. Obtenemos la conexión y limpiamos espacios
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")?.Trim();

// 2. Traducimos la URL al idioma de Npgsql
if (!string.IsNullOrEmpty(connectionString) && (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://")))
{
    var databaseUri = new Uri(connectionString);
    var userInfo = databaseUri.UserInfo.Split(':');

    // CORRECCIÓN: Si no hay puerto en la URL (Render devuelve -1), usamos el estándar 5432
    var port = databaseUri.Port > 0 ? databaseUri.Port : 5432;

    connectionString = $"Host={databaseUri.Host};Port={port};Database={databaseUri.LocalPath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Prefer;Trust Server Certificate=true;";
}

// 3. Inyectamos la base de datos
builder.Services.AddDbContext<SampleDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// --- BLOQUE AÑADIDO: CREACIÓN AUTOMÁTICA DE TABLAS ---
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SampleDbContext>();
    dbContext.Database.Migrate();
}
// -----------------------------------------------------

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 2. ACTIVAR LA SEGURIDAD (EL ORDEN ES VITAL)
app.UseAuthentication(); // <--- ¿Quién eres?
app.UseAuthorization();  // <--- ¿Tienes permiso?

// Map controller routes and Razor Pages
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();