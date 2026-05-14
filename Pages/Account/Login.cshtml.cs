using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace WebApplication24_02.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // --- VALIDACIÓN ---
            if (Username == "admin" && Password == "1234")
            {
                var claims = new List<Claim>
        {
            // Cambiamos "Usuario" por tu nombre real para que se vea en el Layout
            new Claim(ClaimTypes.Name, "Vanessa-Brayan"),
            new Claim(ClaimTypes.Role, "Administrator")
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToPage("/Index");
            }

            // MENSAJE GENÉRICO: No da pistas de cuáles son las credenciales reales
            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos. Por favor, intente de nuevo.");
            return Page();
        }
    }
}