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

// Register DbContext for DI
// 1. Obtenemos la conexión y ELIMINAMOS espacios o saltos de línea invisibles con Trim()
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")?.Trim();

// 2. Traducimos la URL al idioma de Npgsql
if (!string.IsNullOrEmpty(connectionString) && (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://")))
{
    var databaseUri = new Uri(connectionString);
    var userInfo = databaseUri.UserInfo.Split(':');
    connectionString = $"Host={databaseUri.Host};Port={databaseUri.Port};Database={databaseUri.LocalPath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Prefer;Trust Server Certificate=true;";
}

// 3. Inyectamos la base de datos
builder.Services.AddDbContext<SampleDbContext>(options =>
    options.UseNpgsql(connectionString));


var app = builder.Build();

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
app.UseAuthentication(); // <--- AGREGAR ESTO: ¿Quién eres?
app.UseAuthorization();  // <--- ¿Tienes permiso?

// Map controller routes and Razor Pages
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
