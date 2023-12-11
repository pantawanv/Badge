using Badge.Areas.Identity.Data;
using Badge.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Administration.User
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration Configuration;
        public IndexModel(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _roleManager = roleManager;
            Configuration = configuration;
        }

        public string FNameSort { get; set; }
        public string LNameSort { get; set; }
        public string EMailSort { get; set; }
        public string PhoneSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public string View { get; set; }
        public PaginatedList<ApplicationUser> Users { get; set; }

        public async Task OnGetAsync(string searchString, string? view, string sortOrder, string currrentFilter, int? pageIndex)
        {
            CurrentSort = sortOrder;
            FNameSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("FName_asc") ? "FName_desc" : "FName_asc";
            LNameSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("LName_asc") ? "LName_desc" : "LName_asc";
            PhoneSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("Phone_asc") ? "Phone_desc" : "Phone_asc";
            EMailSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("Email_asc") ? "Email_desc" : "Email_asc";

            string leaderId = _roleManager.Roles.First(r => r.Name == "Leader").Id;
            string managerId = _roleManager.Roles.First(r => r.Name == "Manager").Id;

            View = view;

            if (_context.Users != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currrentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<ApplicationUser> UserIQ = from u in _context.Users select u;

            if (UserIQ.Any())
            {

                {
                    switch (View)
                    {
                        case "leader":
                            UserIQ = from u in _context.Users where _context.UserRoles.Where(r => r.UserId == u.Id && r.RoleId == leaderId).Any() select u;

                            break;
                        case "manager":
                            UserIQ = from u in _context.Users where _context.UserRoles.Where(r => r.UserId == u.Id && r.RoleId == managerId).Any() select u;
                            break;
                        default:
                            UserIQ = from u in _context.Users where _context.UserRoles.Where(r => r.UserId == u.Id && r.RoleId == leaderId).Any() || _context.UserRoles.Where(r => r.UserId == u.Id && r.RoleId == managerId).Any() select u;
                            break;
                    }

                    if (CurrentFilter != null)
                    {
                        UserIQ = from u in UserIQ where (u.FName + " " + u.LName + " ").Contains(CurrentFilter) select u;

                        
                    }
                }


                switch (sortOrder)
                {
                    case "FName_desc":
                        UserIQ = UserIQ.OrderByDescending(u => u.FName);
                        break;
                    case "FName_asc":
                        UserIQ = UserIQ.OrderBy(u => u.FName);
                        break;
                    case "LName_desc":
                        UserIQ = UserIQ.OrderByDescending(u => u.LName);
                        break;
                    case "LName_asc":
                        UserIQ = UserIQ.OrderBy(u => u.LName);
                        break;
                    case "Phone_desc":
                        UserIQ = UserIQ.OrderByDescending(u => u.PhoneNumber);
                        break;
                    case "Phone_asc":
                        UserIQ = UserIQ.OrderBy(u => u.PhoneNumber);
                        break;
                    case "Email_desc":
                        UserIQ = UserIQ.OrderByDescending(u => u.Email);
                        break;
                    case "Email_asc":
                        UserIQ = UserIQ.OrderBy(u => u.Email);
                        break;
                    default:
                        CurrentSort = "FName_asc";
                        UserIQ = UserIQ.OrderBy(u => u.FName);
                        break;
                }

                var pageSize = Configuration.GetValue("PageSize", 4);
                Users = await PaginatedList<ApplicationUser>.CreateAsync(UserIQ.AsNoTracking()
                    , pageIndex ?? 1, pageSize);
            }
        }
    }
}
