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
            MemberSort = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            FNameSort = String.IsNullOrEmpty(sortOrder) ? "FName_desc" : "";
            LNameSort = String.IsNullOrEmpty(sortOrder) ? "LName_desc" : "";
            PhoneSort = String.IsNullOrEmpty(sortOrder) ? "Phone_desc" : "";
            EMailSort = String.IsNullOrEmpty(sortOrder) ? "Email_desc" : "";

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
                parentsIQ = parentsIQ.Where(p => p.LName.Contains(searchString)
                || p.FName.Contains(searchString) || p.Member.FullName.Contains(searchString)|| p.LName.Contains(searchString)
                || p.Phone.Contains(searchString) || p.Email.Contains(searchString)
                );

            }


            switch (sortOrder)
            {
                case "id_desc":
                    parentsIQ = parentsIQ.OrderByDescending(p => p.Id);
                    break;
                case "FName_desc":
                    parentsIQ = parentsIQ.OrderByDescending(p => p.FName);
                    break;
                case "LName_desc":
                    parentsIQ = parentsIQ.OrderByDescending(p => p.LName);
                    break;
                case "Phone_desc":
                    parentsIQ = parentsIQ.OrderByDescending(p => p.Phone);
                    break;
                case "Email_desc":
                    parentsIQ = parentsIQ.OrderByDescending(p => p.Email);
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