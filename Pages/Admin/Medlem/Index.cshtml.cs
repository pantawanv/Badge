using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Badge.Pages.Admin.MemberAdmin
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration Configuration;
        private readonly IMemberService _memberService;

        public IndexModel(IConfiguration configuration, IMemberService memberService)
        {
            Configuration = configuration;
            _memberService = memberService;
        }

        public string FNameSort { get; set; }
        public string LNameSort { get; set; }
        public string GroupSort { get; set; }
        public string SaleSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public bool MyGroups { get; set; }
        public int? PageIndex { get; set; }
        public PaginatedList<Member> Members { get; set; }


        public async Task OnGetAsync(string sortOrder, string searchString, int? pageIndex, bool? myGroups)
        {
            CurrentSort = sortOrder;
            FNameSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("FName_asc") ? "FName_desc" : "FName_asc";
            LNameSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("LName_asc") ? "LName_desc" : "LName_asc";
            GroupSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("Group_asc") ? "Group_desc" : "Group_asc";
            SaleSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("Sale_asc") ? "Sale_desc" : "Sale_asc";
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
                searchString = CurrentFilter;
            }
            PageIndex = pageIndex == null ? 1 : pageIndex;
            CurrentFilter = searchString;


            IQueryable<Member> memberIQ = _memberService.GetMembers();
            if (MyGroups)
            {
                memberIQ = memberIQ.Where(m => m.Group.LeaderId == User.Identity.GetUserId());
            }


            if (!String.IsNullOrEmpty(searchString))
            {
                memberIQ = memberIQ.Where(m => m.User.FName.Contains(searchString)
                || (m.User.FName + " " + m.User.LName).Contains(searchString) || m.Group.Name.Contains(searchString));
            }



            switch (sortOrder)
            {
                case "FName_desc":
                    memberIQ = memberIQ.OrderByDescending(m => m.User.FName);
                    break;
                case "FName_asc":
                    memberIQ = memberIQ.OrderBy(m => m.User.FName);
                    break;
                case "LName_desc":
                    memberIQ = memberIQ.OrderByDescending(m => m.User.LName);
                    break;
                case "LName_asc":
                    memberIQ = memberIQ.OrderBy(m => m.User.LName);
                    break;
                case "Group_desc":
                    memberIQ = memberIQ.OrderByDescending(m => m.Group);
                    break;
                case "Group_asc":
                    memberIQ = memberIQ.OrderBy(m => m.Group);
                    break;
                case "Sale_desc":
                    memberIQ = memberIQ.OrderByDescending(m => m.Sales.Count);
                    break;
                case "Sale_asc":
                    memberIQ = memberIQ.OrderBy(m => m.Sales.Count);
                    break;
                default:
                    memberIQ = memberIQ.OrderBy(m => m.Sales.Count);
                    break;
            }



            var pageSize = Configuration.GetValue("PageSize", 4);
            Members = await PaginatedList<Member>.CreateAsync(memberIQ.AsNoTracking().Include(m => m.Group).Include(m => m.Sales).Include(m => m.User), pageIndex ?? 1, pageSize);



        }

    }
}
