using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication24_02.Data;
using WebApplication24_02.Models;
using System.Threading.Tasks;

namespace WebApplication24_02.Pages.Customers
{
    public class CreateModel : PageModel
    {
        private readonly SampleDbContext _context;
        public CreateModel(SampleDbContext context) => _context = context;

        [BindProperty]
        public Customer Customer { get; set; } = new Customer();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            _context.Customers.Add(Customer);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
