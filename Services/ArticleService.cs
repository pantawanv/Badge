using Badge.Data;
using Badge.Interfaces;
using Badge.Models;

namespace Badge.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _context;

        public ArticleService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Henter artiklen
        public async Task<Article> GetArticleAsync(int id) 
        {
            return await _context.Articles.FindAsync(id);
        }
    }
}
