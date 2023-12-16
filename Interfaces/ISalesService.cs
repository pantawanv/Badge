using Badge.Models;
using Microsoft.EntityFrameworkCore;

namespace Badge.Interfaces
{
    public interface ISalesService
    {
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
    }
}
