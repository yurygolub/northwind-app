using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Northwind.Services.Blogging;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;

namespace Northwind.Services.EntityFrameworkCore.Blogging
{
    public class BloggingService : IBloggingService
    {
        private readonly BloggingContext context;

        public BloggingService(IDesignTimeDbContextFactory<BloggingContext> factory)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this.context = factory.CreateDbContext(null);
        }

        public async IAsyncEnumerable<BlogArticle> GetBlogArticlesAsync(int offset, int limit)
        {
            var blogArticles = this.context.BlogArticles
                    .Skip(offset)
                    .Take(limit)
                    .Select(b => MapBlogArticle(b));

            await foreach (var blogArticle in blogArticles.AsAsyncEnumerable())
            {
                yield return blogArticle;
            }
        }

        public async Task<BlogArticle> GetBlogArticleAsync(int blogArticleId)
        {
            var blogArticleEntity = await this.context.BlogArticles.FindAsync(blogArticleId);
            if (blogArticleEntity is null)
            {
                return null;
            }

            return MapBlogArticle(blogArticleEntity);
        }

        public async Task<int> CreateBlogArticleAsync(BlogArticle blogArticle)
        {
            if (blogArticle is null)
            {
                throw new ArgumentNullException(nameof(blogArticle));
            }

            Entities.BlogArticle blogArticleEntity = MapBlogArticle(blogArticle);

            blogArticleEntity.Posted = DateTime.Now;

            await this.context.BlogArticles.AddAsync(blogArticleEntity);
            await this.context.SaveChangesAsync();

            return blogArticleEntity.Id;
        }

        public async Task<bool> DeleteBlogArticleAsync(int blogArticleId)
        {
            var blogArticle = await this.context.BlogArticles.FindAsync(blogArticleId);
            if (blogArticle != null)
            {
                this.context.BlogArticles.Remove(blogArticle);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateBlogArticleAsync(int blogArticleId, BlogArticle blogArticle)
        {
            if (blogArticle is null)
            {
                throw new ArgumentNullException(nameof(blogArticle));
            }

            var blogArticleEntity = await this.context.BlogArticles.FindAsync(blogArticleId);
            if (blogArticleEntity is null)
            {
                return false;
            }

            blogArticleEntity.Title = blogArticle.Title;
            blogArticleEntity.Text = blogArticle.Text;
            blogArticleEntity.Posted = DateTime.Now;

            await this.context.SaveChangesAsync();
            return true;
        }

        private static BlogArticle MapBlogArticle(Entities.BlogArticle blogArticle)
        {
            return new BlogArticle()
            {
                Posted = blogArticle.Posted,
                AuthorId = blogArticle.AuthorId,
                Title = blogArticle.Title,
                Id = blogArticle.Id,
                Text = blogArticle.Text,
            };
        }

        private static Entities.BlogArticle MapBlogArticle(BlogArticle blogArticle)
        {
            return new Entities.BlogArticle()
            {
                Posted = blogArticle.Posted,
                AuthorId = blogArticle.AuthorId,
                Title = blogArticle.Title,
                Id = blogArticle.Id,
                Text = blogArticle.Text,
            };
        }
    }
}
