using Badge.Models;

namespace Badge.Interfaces
{
    public interface IArticleService
    {
        Task<Article> GetArticleAsync(int id);  
    }
}
