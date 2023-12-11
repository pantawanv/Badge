using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Administration.TicketAdmin
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

        public async Task OnGetAsync(string sortOrder, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;

            TicketSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("id_asc") ? "id_desc" : "id_asc";
            GroupNameSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("group_asc") ? "group_desc" : "group_asc";
            MemberNameSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("member_asc") ? "member_desc" : "member_asc";
            SoldSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("sold_asc") ? "sold_desc" : "sold_asc"; 


            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Ticket> ticketsIQ = from t in _context.Tickets
                                           select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                ticketsIQ = ticketsIQ.Where(t => t.Id.Contains(searchString) || t.TicketGroupAssign.Group.Name.Contains(searchString) 
                || (t.TicketMemberAssign.Member.FName + " " + t.TicketMemberAssign.Member.LName + " ").Contains(searchString));
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
                    ticketsIQ = ticketsIQ.OrderByDescending(t => t.TicketMemberAssign.Member.FName);
                    break;
                case "member_asc":
                    ticketsIQ = ticketsIQ.OrderBy(t => t.TicketMemberAssign.Member.FName);
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

            var pageSize = Configuration.GetValue("PageSize", 4);
            var sales = from s in _context.Sales select s;
            Sales = sales.Include(s => s.Ticket);
            Tickets = await PaginatedList<Ticket>.CreateAsync(ticketsIQ.AsNoTracking().Include(t => t.TicketGroupAssign).ThenInclude(t => t.Group).Include(t => t.TicketMemberAssign).ThenInclude(t => t.Member), pageIndex ?? 1, pageSize);

        }
    }
}


