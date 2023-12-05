using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Badge.Data;
using Badge.Models;
using Microsoft.CodeAnalysis.Operations;

namespace Badge.Pages.Administration.MemberAdmin
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

        public string FNameSort { get; set; }
        public string LNameSort { get; set; }
        public string GroupSort { get; set; }
        public string SaleSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Member> Members { get;set; } 


        public async Task OnGetAsync(string sortOrder,string CurrenFilter, string searchString, int? pageIndex)
        {
            FNameSort = String.IsNullOrEmpty(sortOrder) ? "FName_desc" : "";
            LNameSort = String.IsNullOrEmpty(sortOrder) ? "LName_desc" : "";
            GroupSort = String.IsNullOrEmpty(sortOrder) ? "Group_desc" : "";
            SaleSort = String.IsNullOrEmpty(sortOrder) ? "Sale_desc" : "";
           
            if (searchString != null)
            {
                pageIndex = 1; 
            }
            else
            {
                searchString = CurrenFilter;
            }

            CurrenFilter = searchString;

            IQueryable<Member> memberIQ = from m in _context.Members
                                          select m;


            if(!String.IsNullOrEmpty(searchString))
            {
                memberIQ = memberIQ.Where(m => m.FName.Contains(searchString)
                ||m.LName.Contains(searchString) || m.Group.Name.Contains(searchString));
            }
            


            switch (sortOrder)
            {
                case "FName_desc":
                    memberIQ = memberIQ.OrderByDescending(m => m.FName);
                    break;
                case "LName_desc":
                    memberIQ = memberIQ.OrderByDescending(m => m.LName);
                    break;
                case "Group_desc":
                    memberIQ = memberIQ.OrderByDescending(m => m.Group);
                    break;
                case "Sale_desc":
                    memberIQ = memberIQ.OrderByDescending(m => m.Sales.Count);
                    break;
                default:
                    memberIQ = memberIQ.OrderBy(m => m.Sales.Count);
                    break;
            }



            var pageSize = Configuration.GetValue("PageSize", 4);
            Members = await PaginatedList<Member>.CreateAsync(memberIQ.AsNoTracking().Include(m => m.Group).Include(m => m.Sales), pageIndex ?? 1, pageSize);


           
        }
      
    }
}
