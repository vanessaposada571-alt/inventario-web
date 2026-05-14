using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication24_02.Data;
using WebApplication24_02.Models;
using System.Threading.Tasks;

namespace WebApplication24_02.Pages.Customers
{
    public class DeleteModel : PageModel
    {
        private readonly SampleDbContext _context;
        public DeleteModel(SampleDbContext context) => _context = context;

        [BindProperty]
        public Customer Customer { get; set; } = new Customer();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Customer = await _context.Customers.FindAsync(id);
            if (Customer == null)
                return RedirectToPage("Index");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var customer = await _context.Customers.FindAsync(Customer.CustomerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}
