using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Badge.Data;
using Badge.Models;
using Microsoft.AspNet.Identity;

namespace Badge.Pages.Administration.GroupAdmin
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

       public string GroupNameSort { get; set; }
       public string GroupIdSort { get; set; }
       public string GroupLeaderSort { get; set; }
       public string CurrentFilter { get; set; }  
       public string CurrentSort { get; set; }

        public bool MyGroups { get; set; }
       

        public PaginatedList<Group> Groups { get;set; } 

        public async Task OnGetAsync(string sortOrder, string CurrentFilter, string searchString, int? pageIndex, bool myGroups)
        {
            CurrentSort = sortOrder;
            GroupNameSort = String.IsNullOrEmpty(sortOrder) ? "groupName_desc" : "";
            GroupIdSort = String.IsNullOrEmpty(sortOrder) ? "groupId_desc" : "";
            GroupLeaderSort = String.IsNullOrEmpty(sortOrder) ? "groupLeader_desc" : "";
            MyGroups = myGroups;
            if (searchString != null)
            {
                pageIndex = 1;

            }
            else
            {
                searchString = CurrentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Group> groupsIQ = from g in _context.Groups
                                         select g;
            if (MyGroups)
            {
                groupsIQ = groupsIQ.Where(g => g.LeaderId == User.Identity.GetUserId());
            }

            if(!String.IsNullOrEmpty(searchString))
            {
                groupsIQ = groupsIQ.Where(g => g.Name.Contains(searchString) 
                || g.GroupType.Name.Contains(searchString) 
                || g.Leader.FName.Contains(searchString) 
                || g.Leader.LName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "groupName_desc":
                    groupsIQ = groupsIQ.OrderByDescending(g => g.Name);
                    break;
                case "groupId_desc":
                    groupsIQ = groupsIQ.OrderByDescending(g => g.GroupTypeId);
                    break;
                case "groupLeader_desc":
                    groupsIQ = groupsIQ.OrderByDescending(g => g.LeaderId);
                    break;
                default:
                    groupsIQ = groupsIQ.OrderBy(g =>  g.Name);
                    break;
                    
            }

            var pageSize = Configuration.GetValue("PageSize", 4);
            Groups = await PaginatedList<Group>.CreateAsync(groupsIQ.AsNoTracking()
                .Include(g => g.GroupType)
                .Include(g => g.Leader)
                , pageIndex ?? 1, pageSize);

           


          
        }
    }
}
