using Badge.Areas.Identity.Data;
using Badge.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public string PhoneSort {  get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public PaginatedList<ApplicationUser> User { get; set; } 
        public IList<ApplicationUser> Users { get; set; }

        public async Task OnGetAsync(string searchString, string? view, string sortOrder, string currrenFilter, int? pageIndex)
        {

            CurrentSort = sortOrder;
            FNameSort = String.IsNullOrEmpty(sortOrder) ? "FName_desc" : "";
            LNameSort = String.IsNullOrEmpty(sortOrder) ? "LName_desc" : "";
            PhoneSort = String.IsNullOrEmpty(sortOrder) ? "Phone_desc" : "";
            EMailSort = String.IsNullOrEmpty(sortOrder) ? "Email_desc" : "";



            string leaderId = _roleManager.Roles.First(r => r.Name == "Leader").Id;
            string managerId = _roleManager.Roles.First(r => r.Name == "Manager").Id;

            string View = view;
            if (_context.Users != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currrenFilter;
            }

            CurrentFilter = searchString;

            {
                switch (View)
                {
                    case "leader":
                        Users = await (from u in _context.Users where _context.UserRoles.Where(r => r.UserId == u.Id && r.RoleId == leaderId).Any() select u).ToListAsync();

                        break;
                    case "manager":
                        Users = await (from u in _context.Users where _context.UserRoles.Where(r => r.UserId == u.Id && r.RoleId == managerId).Any() select u).ToListAsync();
                        break;
                    default:
                        Users = await (from u in _context.Users where _context.UserRoles.Where(r => r.UserId == u.Id && r.RoleId == leaderId).Any() || _context.UserRoles.Where(r => r.UserId == u.Id && r.RoleId == managerId).Any() select u).ToListAsync();
                        break;
                }



                if (Users != null)
                {
                    if (CurrentFilter != null)
                    {
                        Users = (from u in Users where u.FullName.ToUpper().Contains(CurrentFilter.ToUpper()) select u).ToList();
                    }
                }
            }

          

          

            switch (sortOrder)
            {
                case "FName_desc":
                    Users = Users.AsQueryable().OrderByDescending(u => u.FName).ToList();
                    break;
                case "LName_desc":
                    Users = (IList<ApplicationUser>?)Users.OrderByDescending(u => u.LName);
                    break;
                case "Phone_desc":
                    Users = (IList<ApplicationUser>?)Users.OrderByDescending(u => u.PhoneNumber);
                    break;
                case "EMail_desc":
                    Users = (IList<ApplicationUser>?)Users.OrderByDescending(u => u.Email);
                    break;
                default:
                    Users = Users.AsQueryable().OrderByDescending(u => u.FName).ToList();
                    break;
            }

            var pageSize = Configuration.GetValue("PageSize", 4);
            User = await PaginatedList<ApplicationUser>.CreateAsync(Users.AsQueryable()
                ,pageIndex ?? 1, pageSize);
         
        }
    }
}
