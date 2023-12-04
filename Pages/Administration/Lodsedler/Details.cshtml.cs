using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace Badge.Pages.Administration.TicketAdmin
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;

        }

        public Ticket Ticket { get; set; } = default!;
        [BindProperty]
        public TicketGroupAssign TicketGroupAssign { get; set; }
        public TicketMemberAssign? TicketMemberAssign { get; set; } = default!;
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

                var ticketGroupAssign = await _context.TicketGroupAssigns.FirstOrDefaultAsync(t => t.TicketId == id);
                if (ticketGroupAssign != null)
                {
                    TicketGroupAssign = ticketGroupAssign;
                }
                var ticketMemberAssign = await _context.TicketMemberAssigns.FirstOrDefaultAsync(t => t.TicketId == id);
                if (ticketMemberAssign != null)
                {
                    TicketMemberAssign = ticketMemberAssign;
                }
                var sale = await _context.Sales.Include(t => t.Seller).ThenInclude(t=>t.Group).Include(t => t.Channel).FirstOrDefaultAsync(t=> t.TicketId == id);
                if (sale != null)
                {
                    Sale = sale;
                    
                }
            }
            
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name"); 
            
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
                
                TicketGroupAssign.TicketId = Ticket.Id;
                var result = await _context.TicketGroupAssigns.AddAsync(TicketGroupAssign);
                _context.SaveChanges();
                return RedirectToPage("./Index");
            
        }
    }
}
