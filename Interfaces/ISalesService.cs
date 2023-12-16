using Microsoft.EntityFrameworkCore;

namespace Badge.Interfaces
{
    public interface ISalesService
    {
        int GetSalesCount();
        int GetTicketsCount();
    }
}
