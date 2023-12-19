using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.TicketAdmin
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration Configuration;


        public IndexModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public string TicketSort { get; set; }
        public string GroupNameSort { get; set; }
        public string MemberNameSort { get; set; }
        public string SoldSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Ticket> Tickets { get; set; }
        public IQueryable<Sale> Sales { get; set; }
        public List<Ticket>? SelectedTickets { get; set; }
        public List<TicketGroupAssign>? GroupAssigns { get; set; }
        public async Task<IActionResult> OnGetAsync(string sortOrder, string searchString, int? pageIndex, string[]? selectedTickets)
        {

            CurrentSort = sortOrder;
            TicketSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("id_asc") ? "id_desc" : "id_asc";
            GroupNameSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("group_asc") ? "group_desc" : "group_asc";
            MemberNameSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("member_asc") ? "member_desc" : "member_asc";
            SoldSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("sold_asc") ? "sold_desc" : "sold_asc";
            GroupAssigns = _context.TicketGroupAssigns.ToList();

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }
            CurrentFilter = searchString;

            IQueryable<Ticket> ticketsIQ = from t in _context.Tickets select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                ticketsIQ = ticketsIQ.Where(t => t.Id.Contains(searchString) || t.TicketGroupAssign.Group.Name.Contains(searchString)
                || (t.TicketMemberAssign.Member.User.FName + " " + t.TicketMemberAssign.Member.User.LName).Contains(searchString));
            }

            switch (sortOrder)
            {
                case "id_desc":
                    ticketsIQ = ticketsIQ.OrderByDescending(t => t.Id);
                    break;
                case "id_asc":
                    ticketsIQ = ticketsIQ.OrderBy(t => t.Id);
                    break;
                case "group_desc":
                    ticketsIQ = ticketsIQ.OrderByDescending(t => t.TicketGroupAssign.Group.Name);
                    break;
                case "group_asc":
                    ticketsIQ = ticketsIQ.OrderBy(t => t.TicketGroupAssign.Group.Name);
                    break;
                case "member_desc":
                    ticketsIQ = ticketsIQ.OrderByDescending(t => t.TicketMemberAssign.Member.User.FName);
                    break;
                case "member_asc":
                    ticketsIQ = ticketsIQ.OrderBy(t => t.TicketMemberAssign.Member.User.FName);
                    break;
                case "sold_desc":
                    ticketsIQ = ticketsIQ.OrderByDescending(t => t.Sale != null);
                    break;
                case "sold_asc":
                    ticketsIQ= ticketsIQ.OrderBy(t => t.Sale != null);
                    break;
                default:
                    ticketsIQ = ticketsIQ.OrderBy(t => t.Id);
                    break;
            }
            if (selectedTickets != null)
            {
                SelectedTickets = new List<Ticket>();
                foreach (string id in selectedTickets)
                {
                    SelectedTickets.Add(_context.Tickets.Include(t => t.TicketGroupAssign).ThenInclude(t => t.Group).ThenInclude(t => t.Members).ThenInclude(t => t.User).FirstOrDefault(t => t.Id == id));
                }
            }

            var groups = await _context.Groups.ToListAsync();
            if (groups != null)
            {
                ViewData["GroupId"] = new SelectList(groups, "Id", "Name");
            }


            var pageSize = Configuration.GetValue("PageSize", 4);
            var sales = from s in _context.Sales select s;
            Sales = sales.Include(s => s.Ticket);
            Tickets = await PaginatedList<Ticket>.CreateAsync(ticketsIQ.AsNoTracking().Include(t => t.TicketGroupAssign).ThenInclude(t => t.Group).Include(t => t.TicketMemberAssign).ThenInclude(t => t.Member).ThenInclude(m => m.User), pageIndex ?? 1, pageSize);
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string[]? selectedTickets)
        {
            if (selectedTickets != null)
            {
                SelectedTickets = new List<Ticket>();
                foreach (string id in selectedTickets)
                {
                    SelectedTickets.Add(_context.Tickets.Include(t => t.TicketGroupAssign).FirstOrDefault(t => t.Id == id));
                }
            }

            foreach (Ticket ticket in SelectedTickets)
            {
                _context.Tickets.Remove(ticket);
            }
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostCreateGroupAssignAsync(string[]? selectedTickets, int selectedGroup)
        {
            if (selectedTickets != null)
            {
                SelectedTickets = new List<Ticket>();
                foreach (string id in selectedTickets)
                {
                    SelectedTickets.Add(_context.Tickets.FirstOrDefault(t => t.Id == id));
                }
                foreach (Ticket ticket in SelectedTickets)
                {
                    if (!_context.TicketGroupAssigns.Where(t => t.TicketId == ticket.Id).Any())
                    {
                        TicketGroupAssign assign = new TicketGroupAssign() { TicketId = ticket.Id, GroupId = selectedGroup };
                        _context.TicketGroupAssigns.Add(assign);
                    }
                }
                await _context.SaveChangesAsync();

            }

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostCreateMemberAssignAsync(string[]? selectedTickets, string selectedMember)
        {
            if (selectedTickets != null)
            {
                SelectedTickets = new List<Ticket>();
                foreach (string id in selectedTickets)
                {
                    SelectedTickets.Add(_context.Tickets.FirstOrDefault(t => t.Id == id));
                }
                foreach (Ticket ticket in SelectedTickets)
                {
                    if (!_context.TicketMemberAssigns.Where(t => t.TicketId == ticket.Id).Any())
                    {
                        TicketMemberAssign assign = new TicketMemberAssign() { TicketId = ticket.Id, MemberId = selectedMember };
                        _context.TicketMemberAssigns.Add(assign);
                    }
                }
                await _context.SaveChangesAsync();

            }

            return RedirectToPage();
        }

        public bool CanAssingMember()
        {
            if (CanAssign()
                && !CanAssignGroup()
                && SelectedTickets.Where(t => t.TicketGroupAssign != null).Count() == SelectedTickets.Count()
                && SelectedTickets.Where(t => t.TicketGroupAssign.GroupId != (SelectedTickets.ElementAtOrDefault(0).TicketGroupAssign.GroupId)).Any() == false
                && SelectedTickets.Where(t => t.TicketMemberAssign != null).Any() == false)

            {

                ViewData["MemberId"] = new SelectList(SelectedTickets.ElementAt(0).TicketGroupAssign.Group.Members, "Id", "User.FullName");
                return true;

            }
            return false;

        }

        public bool CanAssignGroup()
        {
            return CanAssign() && SelectedTickets.Where(t => t.TicketGroupAssign != null).Any() == false;
        }

        public bool CanAssign()
        {
            return SelectedTickets.Any();
        }
    }
}


