using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Badge.Pages.Admin.SalesAdmin
{
    public class CreateModel : PageModel
    {
        private readonly ISalesService _salesService;
        private readonly IMemberService _memberService;

        public CreateModel(ISalesService salesService, IMemberService memberService)
        {
            _salesService = salesService;
            _memberService = memberService;
        }

        public Ticket SelectedTicket { get; set; }

        [BindProperty]
        public Sale Sale { get; set; } = default!;


        public List<Ticket> Tickets { get; set; }
        public async Task<IActionResult> OnGetAsync(string? selected)
        {
            var tickets = await _salesService.GetAvailableTicketsAsync();
            if (tickets != null)
            {
                Tickets = tickets;
            }

            if (selected != null)
            {
                SelectedTicket = await _salesService.GetTicketAsync(selected);
                if (SelectedTicket==null)
                {
                    return NotFound();
                }
                var member = await _salesService.GetAssignedMemberAsync(selected);
                var group = await _salesService.GetAssignedGroupAsync(selected);
                if (member == null)
                {
                    if (group == null)
                    {
                        var members = await _memberService.GetAllMembersAsync();
                        ViewData["MemberId"] = new SelectList(members, "Id", "User.FullName");
                    }
                    else
                    {

                        var members = await _memberService.GetAllMembersOfGroupAsync(group.Id);
                        ViewData["MemberId"] = new SelectList(members, "Id", "User.FullName");
                    }
                }
                else
                {
                    member = await _salesService.GetAssignedMemberAsync(selected);
                    ViewData["MemberId"] = new SelectList(member.Id, member.User.FullName);
                }
                ViewData["TicketId"] =  new SelectList(selected);
                var channels = _salesService.GetChannels();
                ViewData["ChannelId"] = new SelectList(channels, "Id", "Name");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _salesService.AddSaleAsync(Sale);
            return RedirectToPage("./Index");
        }
    }
}
