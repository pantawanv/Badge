using Badge.Areas.Identity.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol.Plugins;
using System.Linq;

namespace Badge.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly ISalesService _salesService;
        private readonly IMemberService _memberService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGroupService _groupService;
        public IndexModel(ISalesService salesService, IMemberService memberService, UserManager<ApplicationUser> userManager, IGroupService groupService) 
        {
            _salesService = salesService;
            _memberService = memberService;
            _userManager = userManager;
            _groupService = groupService;
        }

        public IEnumerable<Member> TopSellers;
        public IEnumerable<Member> TopSellersLeaderGroup;
        public async Task<IActionResult> OnGetAsync()
        {
            var members = _memberService.GetMembers();
            if(members == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var leaderGroups = await _groupService.GetGroupsByLeaderIdAsync(user.Id);
            if (User.IsInRole("Leader") && leaderGroups.Any())
            {
                var leadergroupmembers = new List<Member>();
                
                foreach(var group in leaderGroups)
                {
                    foreach(var member in group.Members)
                    {
                        leadergroupmembers.Add(member);
                    }
                }
                var sortedleadergroupmembers = leadergroupmembers.OrderByDescending( m => m.Sales.Count()).Take(5);
                TopSellersLeaderGroup = sortedleadergroupmembers;
            }
            var sortedMembers = members.OrderByDescending(m => m.Sales.Count()).Take(5);
            

            TopSellers = sortedMembers;
            return Page();
            
        }
    }
}
