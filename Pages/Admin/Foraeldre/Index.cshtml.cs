using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.ParentAdmin
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration Configuration;
        private readonly IMemberService _memberService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, IConfiguration configuration, IMemberService memberService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            Configuration = configuration;
            _memberService = memberService;
            _userManager = userManager;

        }

        public string MemberSort { get; set; }
        public string FNameSort { get; set; }
        public string LNameSort { get; set; }
        public string PhoneSort { get; set; }
        public string EMailSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public int? PageIndex { get; set; }
        public bool MyGroups { get; set; }


        public PaginatedList<Parent> Parents { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex, bool? myGroups)
        {
            CurrentSort = sortOrder;
            MemberSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("memberName_asc") ? "memberName_desc" : "memberName_asc";
            FNameSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("FName_asc") ? "FName_desc" : "FName_asc";
            LNameSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("LName_asc") ? "LName_desc" : "LName_asc";
            PhoneSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("Phone_asc") ? "Phone_desc" : "Phone_asc";
            EMailSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("Email_asc") ? "Email_desc" : "Email_asc";
            if (myGroups == null && User.IsInRole("Leader") || myGroups == true)
            {
                MyGroups = true;
            }
            else
            {
                MyGroups=false;
            }

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            PageIndex = pageIndex == null ? 1 : pageIndex;
            CurrentFilter = searchString;

            IQueryable<Parent> parentsIQ = _memberService.GetParentsQuery();
            if (MyGroups)
            {
                var userId = _userManager.GetUserId(User);
                parentsIQ = parentsIQ.Where(p => p.Members.Any(m => m.Member.Group.LeaderId == userId));
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                parentsIQ = parentsIQ.Where(p => (p.FName + " " + p.LName).Contains(searchString)
                || p.Phone.Contains(searchString) || p.Email.Contains(searchString) || p.Members.Where(m => (m.Member.User.FName + " " + m.Member.User.LName).Contains(searchString)).Any()
                );

            }


            switch (sortOrder)
            {
                case "memberName_desc":
                    parentsIQ = parentsIQ.OrderByDescending(p => p.Members.First().Member.User.FName);
                    break;
                case "memberName_asc":
                    parentsIQ = parentsIQ.OrderBy(p => p.Members.First().Member.User.FName);
                    break;
                case "FName_desc":
                    parentsIQ = parentsIQ.OrderByDescending(p => p.FName);
                    break;
                case "FName_asc":
                    parentsIQ = parentsIQ.OrderBy(p => p.FName);
                    break;
                case "LName_desc":
                    parentsIQ = parentsIQ.OrderByDescending(p => p.LName);
                    break;
                case "LName_asc":
                    parentsIQ = parentsIQ.OrderBy(p => p.LName);
                    break;
                case "Phone_desc":
                    parentsIQ = parentsIQ.OrderByDescending(p => p.Phone);
                    break;
                case "Phone_asc":
                    parentsIQ = parentsIQ.OrderBy(p => p.Phone);
                    break;
                case "Email_desc":
                    parentsIQ = parentsIQ.OrderByDescending(p => p.Email);
                    break;
                case "Email_asc":
                    parentsIQ = parentsIQ.OrderBy(p => p.Email);
                    break;
                default:
                    parentsIQ = parentsIQ.OrderBy(p => p.FName);
                    break;
            }



            var pageSize = Configuration.GetValue("PageSize", 4);
            Parents = await PaginatedList<Parent>.CreateAsync(parentsIQ.AsNoTracking().Include(p => p.Members).ThenInclude(m => m.Member).ThenInclude(p=>p.Group), pageIndex ?? 1, pageSize);



        }
    }
}