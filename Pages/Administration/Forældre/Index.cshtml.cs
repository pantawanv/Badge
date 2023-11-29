using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Badge.Data;
using Badge.Models;
using System.Text.RegularExpressions;

namespace Badge.Pages.Administration.Forældre
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string MemberSort { get; set; }
        public string FNameSort { get; set; }
        public string CurrentFilter { get; set; }
       

        public IList<Parent> Parents { get; set; }

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            MemberSort = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            FNameSort = String.IsNullOrEmpty(sortOrder) ? "FName_desc" : "";

            CurrentFilter = searchString;
        

            IQueryable<Parent> parentsIQ = from p in _context.Parents
                                           select p;

            if (!String.IsNullOrEmpty(searchString) )
            {
                parentsIQ = parentsIQ.Where(p => p.LName.Contains(searchString)
                || p.FName.Contains(searchString) || p.Member.FName.Contains(searchString));

            }


            switch (sortOrder)
            {
                case "id_desc":
                    parentsIQ = parentsIQ.OrderByDescending(p => p.Id);
                    break;
                case "FName_desc":
                    parentsIQ = parentsIQ.OrderByDescending(p => p.FName);
                    break;
                default:
                    parentsIQ = parentsIQ.OrderBy(p => p.FName);
                    break;
                

            }

            if (_context.Parents != null)
            {
                Parents = await parentsIQ.AsNoTracking()
                .Include(p => p.Member).ToListAsync();
            
            }

          
          
        }
    }
}