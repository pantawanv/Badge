using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.GroupAdmin
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IGroupService _groupService;
        private readonly ISalesService _salesService;

        public DetailsModel(ApplicationDbContext context, ISalesService salesService, IGroupService groupService)
        {
            _context = context;
            _salesService=salesService;
            _groupService=groupService;
        }

        public Group Group { get; set; } = default!;
        public List<Member> Members { get; set; } = default!;
        public List<TicketGroupAssign> TicketGroups { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }
            
            var group = await _context.Groups.Include(g => g.Leader).Include(g => g.GroupType).FirstOrDefaultAsync(m => m.Id == id);
            if (group == null)
            {
                return NotFound();
            }
            
                Group = group;
                var members = from m in _context.Members where m.GroupId == Group.Id select m;
                Members = await members.AsNoTracking().Include(m => m.Sales).Include(m => m.User).ToListAsync();
                
            var tickets = await _context.TicketGroupAssigns.Include(t=>t.Ticket).ThenInclude(t =>t.TicketMemberAssign).ThenInclude(t=>t.Member).ThenInclude(t=>t.User).Include(t=>t.Ticket).ThenInclude(t=>t.Sale).ThenInclude(t=>t.Seller).Where(t => t.GroupId == Group.Id).ToListAsync();
            TicketGroups = tickets;
            return Page();
        }
    }
}
