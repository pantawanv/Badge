using Badge.Data;
using Badge.Interfaces;
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
        private readonly ISalesService _salesService;
        private readonly IMemberService _memberService;

        public DetailsModel(ApplicationDbContext context, ISalesService salesService, IMemberService memberService)
        {
            _context = context;
            _salesService = salesService;
            _memberService=memberService;
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

            var ticket = await _salesService.GetTicketAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }
            else
            {
                Ticket = ticket;

                var ticketGroupAssign = await _salesService.GetTicketGroupAssignAsync(id);
                if (ticketGroupAssign != null)
                {
                    TicketGroupAssign = ticketGroupAssign;
                    var groupMembers = await _memberService.GetAllMembersOfGroupAsync(ticketGroupAssign.GroupId);
                    ViewData["GroupMembers"] = new SelectList(groupMembers, "Id", "User.FullName");
                }
                var ticketMemberAssign = await _salesService.GetTicketMemberAssignAsync(id);
                if (ticketMemberAssign != null)
                {
                    TicketMemberAssign = ticketMemberAssign;
                }
                var sale = await _salesService.GetTicketSaleAsync(id);
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
            await _salesService.DeleteTicketGroupAssignAsync(Ticket.Id);
            return RedirectToPage("Details", new { id = Ticket.Id });
        }

        public async Task<IActionResult> OnPostDeleteMemberAssignAsync()
        {
            await _salesService.DeleteTicketMemberAssignAsync(Ticket.Id);
            return RedirectToPage("Details", new { id = Ticket.Id });
        }

        public async Task<IActionResult> OnPostAddAssignAsync()
        {
            var ticketGroupAssign = await _salesService.GetTicketGroupAssignAsync(Ticket.Id);
            if (ticketGroupAssign == null)
            {
                await _salesService.AddTicketGroupAssignAsync(Ticket.Id, TicketGroupAssign.GroupId);
            }
            else
            {
                var memberGroupAssign = await _salesService.GetTicketMemberAssignAsync(Ticket.Id);
                if (memberGroupAssign == null)
                {
                    await _salesService.AddTicketMemberAssignAsync(Ticket.Id, TicketMemberAssign.MemberId);
                }
            }
            return RedirectToPage("Details", new { id = Ticket.Id });
        }
    }
}
