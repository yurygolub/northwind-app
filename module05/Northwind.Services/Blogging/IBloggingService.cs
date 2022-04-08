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

        IAsyncEnumerable<int> GetAllRelatedProductIdsAsync(int articleId, int offset, int limit);

        Task<int> CreateProductLinkAsync(int articleId, int productId);

        Task<bool> RemoveProductLinkAsync(int articleId, int productId);
    }
}
