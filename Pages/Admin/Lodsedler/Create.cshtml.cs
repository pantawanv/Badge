using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Badge.Pages.Admin.TicketAdmin
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(bool multiInsert)
        {
            if (multiInsert)
            {
                MultiInsert = true;
            }
            else { MultiInsert = false; }

            return Page();
        }

        [BindProperty]
        public Ticket Ticket { get; set; } = default!;

        [BindProperty]
        public string? MultiTicket { get; set; } = default!;
        public bool? MultiInsert { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if ( _context.Tickets == null || Ticket == null)
            {
                return Page();
            }

            if(MultiTicket != null)
            {
               var items = MultiTicket.Split(' ');
                foreach (var item in items)
                {
                    var ticket = new Ticket();
                    ticket.Id = item.ToString();

                    if (_context.Tickets.Where(t => t.Id == ticket.Id).Any() && item != "" && item != " ")
                    {
                        ModelState.AddModelError("Ticket.Id", "En lodseddel med følgende id: " + ticket.Id + " findes allerede.");
                        return Page();
                    }
                    _context.Tickets.Add(ticket);
                    await _context.SaveChangesAsync();
                }
            } else
            {
                if (_context.Tickets.Where(t => t.Id == Ticket.Id).Any())
                {
                    ModelState.AddModelError("Ticket.Id", "En lodseddel med følgende id: " + Ticket.Id + " findes allerede.");
                    return Page();
                }

                _context.Tickets.Add(Ticket);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}
