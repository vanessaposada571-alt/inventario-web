using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication24_02.Data;
using WebApplication24_02.Models;
using System.Threading.Tasks;

namespace WebApplication24_02.Pages.Customers
{
    public class DetailsModel : PageModel
    {
        private readonly SampleDbContext _context;
        public DetailsModel(SampleDbContext context) => _context = context;

        public Customer Customer { get; set; } = new Customer();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Customer = await _context.Customers.FindAsync(id);
            if (Customer == null)
                return RedirectToPage("Index");
            return Page();
        }
    }
}
