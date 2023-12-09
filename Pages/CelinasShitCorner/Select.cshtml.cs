using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.CelinasShitCorner
{
    public class SelectModel : PageModel
    {
        public readonly ApplicationDbContext _context;
        public SelectModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Member> Members { get; set; }
        public IList<Member> SelectedProducts { get; set; }
        public async Task<IActionResult> OnGetAsync(int[]? selectedProducts)
        {
            var members = from m in _context.Members select m;
            selectedProducts = selectedProducts ?? new int[0];
            Members = await members.Include(m => m.Group).ToListAsync();
            var select = from m in Members where (selectedProducts.Contains(m.Id))select m;
            SelectedProducts = select.ToList();
            return Page();
        }
    }
}
