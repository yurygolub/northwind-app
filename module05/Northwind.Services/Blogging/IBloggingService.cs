using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Services.Blogging
{
    public interface IBloggingService
    {
        IAsyncEnumerable<BlogArticle> GetBlogArticlesAsync(int offset, int limit);

        Task<BlogArticle> GetBlogArticleAsync(int blogArticleId);

        Task<int> CreateBlogArticleAsync(BlogArticle blogArticle);

        Task<bool> DeleteBlogArticleAsync(int blogArticleId);

        Task<bool> UpdateBlogArticleAsync(int blogArticleId, BlogArticle blogArticle);
    }
}
