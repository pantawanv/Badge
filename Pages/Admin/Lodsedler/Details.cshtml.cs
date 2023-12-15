using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.TicketAdmin
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;

        }
        [BindProperty]
        public Ticket Ticket { get; set; } = default!;
        [BindProperty]
        public TicketGroupAssign TicketGroupAssign { get; set; }
        [BindProperty]
        public TicketMemberAssign TicketMemberAssign { get; set; }
        public Sale? Sale { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FirstOrDefaultAsync(m => m.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }
            else
            {
                Ticket = ticket;

                var ticketGroupAssign = await _context.TicketGroupAssigns.Include(t => t.Group).FirstOrDefaultAsync(t => t.TicketId == id);
                if (ticketGroupAssign != null)
                {
                    TicketGroupAssign = ticketGroupAssign;
                    var groupMembers = _context.Members.Include(m => m.User).Where(m => m.GroupId == ticketGroupAssign.GroupId);
                    ViewData["GroupMembers"] = new SelectList(groupMembers, "Id", "User.FullName");
                }
                var ticketMemberAssign = await _context.TicketMemberAssigns.Include(t => t.Member).ThenInclude(t => t.User).FirstOrDefaultAsync(t => t.TicketId == id);
                if (ticketMemberAssign != null)
                {
                    TicketMemberAssign = ticketMemberAssign;
                }
                var sale = await _context.Sales.Include(t => t.Seller).ThenInclude(t => t.Group).Include(t => t.Channel).FirstOrDefaultAsync(t => t.TicketId == id);
                if (sale != null)
                {
                    Sale = sale;

                }
            }

            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name");


            return Page();
        }

        public async Task<IActionResult> OnPostDeleteGroupAssignAsync()
        {
            TicketGroupAssign ticketGroupAssign = await _context.TicketGroupAssigns.FirstOrDefaultAsync(t => t.TicketId == Ticket.Id);

            if (ticketGroupAssign != null)
            {

                _context.TicketGroupAssigns.Remove(ticketGroupAssign);
                _context.SaveChanges();

                await OnPostDeleteMemberAssignAsync();
            }

            return RedirectToPage("Details", new { id = Ticket.Id });
        }

        public async Task<IActionResult> OnPostDeleteMemberAssignAsync()
        {
            TicketMemberAssign ticketMemberAssign = await _context.TicketMemberAssigns.FirstOrDefaultAsync(t => t.TicketId == Ticket.Id);
            if (ticketMemberAssign != null)
            {
                _context.TicketMemberAssigns.Remove(ticketMemberAssign);
                _context.SaveChanges();
            }
            return RedirectToPage("Details", new { id = Ticket.Id });
        }

        public async Task<IActionResult> OnPostAddAssignAsync()
        {
            var ticketGroupAssign = await _context.TicketGroupAssigns.Include(t => t.Group).FirstOrDefaultAsync(t => t.TicketId == Ticket.Id);
            if (ticketGroupAssign == null)
            {
                TicketGroupAssign.TicketId = Ticket.Id;
                var result = await _context.TicketGroupAssigns.AddAsync(TicketGroupAssign);
                _context.SaveChanges();
            }
            else
            {
                var memberGroupAssign = await _context.TicketMemberAssigns.FirstOrDefaultAsync(t => t.TicketId == Ticket.Id);
                if (memberGroupAssign == null)
                {
                    TicketMemberAssign.TicketId = Ticket.Id;
                    var result = await _context.TicketMemberAssigns.AddAsync(TicketMemberAssign);
                    _context.SaveChanges();
                }
            }
            return RedirectToPage("Details", new { id = Ticket.Id });
        }
    }
}
