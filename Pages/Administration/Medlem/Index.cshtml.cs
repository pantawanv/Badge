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

namespace Badge.Pages.Administration.Medlem
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string SaleSort { get; set; }
        public string FNameSort { get; set; }
        public string CurrentFilter { get; set; }

      

        public IList<Models.Member> Members { get;set; } 


        public async Task OnGetAsync(string sortOrder, string searchString)
        {


            SaleSort = String.IsNullOrEmpty(sortOrder) ? "sale_desc" : "";
            FNameSort = String.IsNullOrEmpty(sortOrder) ? "FName_desc" : "";

            CurrentFilter = searchString;

            IQueryable<Models.Member> memberIQ = from m in _context.Members
                                          select m;

            if(!String.IsNullOrEmpty(searchString))
            {
                memberIQ = memberIQ.Where(m => m.FName.Contains(searchString));
            }
            

            switch (sortOrder)
            {
                case "sale_desc":
                    memberIQ = memberIQ.OrderByDescending(m => CountSales(m));
                    break;

                default:
                    memberIQ = memberIQ.OrderBy(m => CountSales(m));
                    break;
            }



            if (_context.Members != null)
            {
                Members = await memberIQ.AsNoTracking()
                .Include(m => m.Group).ToListAsync();
            }
        }

        public int CountSales(Models.Member member)
        {
            var count = from s in _context.Sales where s.SellerId == member.Id select s;
            return count.Count();
        }
      
    }
}
