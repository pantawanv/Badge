using Badge.Data;
using Badge.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Administration.ParentAdmin
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

        public string MemberSort { get; set; }
        public string FNameSort { get; set; }
        public string LNameSort { get; set; }
        public string PhoneSort { get; set; }
        public string EMailSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }


        public PaginatedList<Parent> Parents { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            MemberSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("memberName_asc") ? "memberName_desc" : "memberName_asc";
            FNameSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("FName_asc") ? "FName_desc" : "FName_asc";
            LNameSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("LName_asc") ? "LName_desc" : "LName_asc";
            PhoneSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("Phone_asc") ? "Phone_desc" : "Phone_asc";
            EMailSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("Email_asc") ? "Email_desc" : "Email_asc";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Parent> parentsIQ = from p in _context.Parents
                                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                parentsIQ = parentsIQ.Where(p => (p.FName + " " + p.LName).Contains(searchString)
                || p.Phone.Contains(searchString) || p.Email.Contains(searchString)
                || (p.Member.FName + " " + p.Member.LName).Contains(searchString)
                );

            }


            switch (sortOrder)
            {
                case "memberName_desc":
                    parentsIQ = parentsIQ.OrderByDescending(p => p.Member.FName);
                    break;
                case "memberName_asc":
                    parentsIQ = parentsIQ.OrderBy(p => p.Member.FName);
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
            Parents = await PaginatedList<Parent>.CreateAsync(parentsIQ.AsNoTracking()
                .Include(p => p.Member), pageIndex ?? 1, pageSize);



        }
    }
}