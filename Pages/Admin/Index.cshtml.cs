using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Badge.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly ISalesService _salesService;
        private readonly IMemberService _memberService;
        public IndexModel(ISalesService salesService, IMemberService memberService) 
        {
            _salesService = salesService;
            _memberService = memberService;
        }

        public IEnumerable<Member> TopSellers;
        public async Task<IActionResult> OnGetAsync()
        {
            var members = _memberService.GetMembers();
            if(members == null)
            {
                return NotFound();
            }
            var sortedMembers = members.OrderByDescending(m => m.Sales.Count()).Take(5);

            TopSellers = sortedMembers;
            return Page();
            
        }
    }
}
