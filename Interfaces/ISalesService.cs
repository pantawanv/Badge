using Badge.Models;
using Microsoft.EntityFrameworkCore;

namespace Badge.Interfaces
{
    public interface ISalesService
    {
        Task<List<Sale>> GetAllSalesAsync();
        IQueryable<Sale> GetAllSalesQuery();
        int GetSalesCount();
        int GetTicketsCount();

        Task<List<Ticket>> GetTicketsAsync();
        Task<List<Ticket>> GetAvailableTicketsAsync();
        Task<Group> GetAssignedGroupAsync(string id);
        Task<Member> GetAssignedMemberAsync(string id);
        Task<Ticket> GetTicketAsync(string id);
        List<Channel> GetChannels();
        Task AddSaleAsync(Sale saleToAdd);
        double GetEstimatedTotalEarnings();
        
        Task<List<Sale>> GetMembersSalesAsync(string id);
        Task<List<Sale>> GetGroupSalesAsync(int id);


        //Ticket Assigns
        //Delete Ticket Group Assign 
        Task DeleteTicketGroupAssignAsync(string id);
        //Delete Ticket Member Assign 
        Task DeleteTicketMemberAssignAsync(string id);
        //Add Ticket Group Assign 
        Task AddTicketGroupAssignAsync(string ticketid, int groupid);
        //Add Ticket Member Assign 
        Task AddTicketMemberAssignAsync(string ticketid, string memberid);

        //Get Ticket Group Assign
        Task<TicketGroupAssign> GetTicketGroupAssignAsync(string ticketid);
        //Get Ticket Member Assign
        Task<TicketMemberAssign> GetTicketMemberAssignAsync(string ticketid);
        //Get Ticket Sale
        Task<Sale> GetTicketSaleAsync(string ticketid);
    }
}
