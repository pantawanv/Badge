using Badge.Areas.Identity.Data;
using Badge.Data;
using Badge.Interfaces;
using Badge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Badge.Pages.Admin.SalesAdmin
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISalesService _salesService;
        private readonly IConfiguration Configuration;

        public IndexModel(ISalesService salesService, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            Configuration = configuration;
            _userManager = userManager;
            _salesService =salesService;
        }

        public string TicketSort { get; set; }
        public string SellerSort { get; set; }
        public string ChannelSort { get; set; }
        public string PaymentCollectedSort { get; set; }
        public string SalesDateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Sale> Sales { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;

            TicketSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("ticket_asc") ? "ticket_desc" : "ticket_asc";
            SellerSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("seller_asc") ? "seller_desc" : "seller_asc";
            ChannelSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("channel_asc") ? "channel_desc" : "channel_asc";
            PaymentCollectedSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("paymentCollected_asc") ? "paymentCollected_desc" : "paymentCollected_asc";
            SalesDateSort = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("salesDate_asc") ? "salesDate_desc" : "salesDate_asc";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;
            IQueryable<Sale> salesIQ = _salesService.GetAllSalesQuery();

            if (User.IsInRole("Leader") && !User.IsInRole("Manager"))
            {
                var user = await _userManager.GetUserAsync(User);
                
            }

            

            if (!String.IsNullOrEmpty(searchString))
            {
                salesIQ = salesIQ.Where(s => s.TicketId.Contains(searchString)
                || (s.Seller.User.FName + " " + s.Seller.User.LName).Contains(searchString));

            }

            switch (sortOrder)
            {
                case "ticket_desc":
                    salesIQ = salesIQ.OrderByDescending(s => s.TicketId);
                    break;
                case "ticket_asc":
                    salesIQ = salesIQ.OrderBy(s => s.TicketId);
                    break;
                case "seller_desc":
                    salesIQ = salesIQ.OrderByDescending(s => s.Seller.User.FName);
                    break;
                case "seller_asc":
                    salesIQ = salesIQ.OrderBy(s => s.Seller.User.FName);
                    break;
                case "channel_desc":
                    salesIQ = salesIQ.OrderByDescending(s => s.Channel);
                    break;
                case "channel_asc":
                    salesIQ = salesIQ.OrderBy(s => s.Channel);
                    break;
                case "paymentCollected_desc":
                    salesIQ = salesIQ.OrderByDescending(s => s.PaymentCollected);
                    break;
                case "paymentCollected_asc":
                    salesIQ = salesIQ.OrderBy(s => s.PaymentCollected);
                    break;
                case "salesDate_desc":
                    salesIQ = salesIQ.OrderByDescending(s => s.SalesDate);
                    break;
                case "salesDate_asc":
                    salesIQ = salesIQ.OrderBy(s => s.SalesDate);
                    break;
                default:
                    salesIQ = salesIQ.OrderBy(s => s.Ticket);
                    break;
            }

            var pageSize = Configuration.GetValue("PageSize", 4);
            Sales = await PaginatedList<Sale>.CreateAsync(salesIQ.AsNoTracking()
                .Include(s => s.Channel)
                .Include(s => s.Seller)
                .ThenInclude(s => s.User)
                .Include(s => s.Ticket), pageIndex ?? 1, pageSize);





        }
    }
}
