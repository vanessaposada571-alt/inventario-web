using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication24_02.Data;
using WebApplication24_02.Models;
using System.Threading.Tasks;

namespace WebApplication24_02.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly SampleDbContext _context;
        public DetailsModel(SampleDbContext context) => _context = context;

        public Order Order { get; set; } = new Order();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.OrderId == id);
            if (Order == null)
                return RedirectToPage("Index");
            return Page();
        }
    }
}
