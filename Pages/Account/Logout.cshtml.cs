using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication24_02.Pages.Account
{
    public class LogoutModel : PageModel
    {
        // DEBE SER OnPostAsync (no OnGet)
        public async Task<IActionResult> OnPostAsync()
        {
            // Borra la cookie del navegador
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Te manda de regreso al Login
            return RedirectToPage("/Account/Login");
        }
    }
}