using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Badge.Data;
using Badge.Models;

namespace Badge.Pages.Administration.GroupAdmin
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string CurrentFilter { get; set; }   

        public IList<Group> Groups { get;set; } 

        public async Task OnGetAsync(string searchString)
        {
            CurrentFilter = searchString;


            IQueryable<Group> groupsIQ = from g in _context.Groups
                                         select g;

            if(!String.IsNullOrEmpty(searchString))
            {
                groupsIQ = groupsIQ.Where(g => g.Name.Contains(searchString));
                
            }


            if (_context.Groups != null)
            {
                Groups = await groupsIQ.AsNoTracking()
                .Include(g => g.GroupType)
                .Include(g => g.Leader).ToListAsync();
            }


          
        }
    }
}
