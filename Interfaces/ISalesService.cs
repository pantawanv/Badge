using Badge.Models;

namespace Badge.Interfaces
{
    public interface ISalesService
    {
        Task<List<Sale>> GetAllSalesAsync();
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
    }
}
