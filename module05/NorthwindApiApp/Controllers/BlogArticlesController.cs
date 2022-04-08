using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;
using NorthwindApiApp.Models;

#pragma warning disable SA1600

namespace NorthwindApiApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogArticlesController : ControllerBase
    {
        private readonly IBloggingService bloggingService;
        private readonly IEmployeeManagementService employeeService;

        public BlogArticlesController(IBloggingService bloggingService, IEmployeeManagementService employeeService)
        {
            this.bloggingService = bloggingService ?? throw new ArgumentNullException(nameof(bloggingService));
            this.employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }

        [HttpGet]
        public async IAsyncEnumerable<BlogArticleShortInfo> GetBlogArticles([FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            await foreach (var blogArticle in this.bloggingService.GetBlogArticlesAsync(offset, limit))
            {
                var author = await this.employeeService.GetEmployeeAsync(blogArticle.AuthorId);
                if (author != null)
                {
                    yield return new BlogArticleShortInfo(blogArticle, author);
                }
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogArticleAsync(int id)
        {
            var blogArticle = await this.bloggingService.GetBlogArticleAsync(id);
            if (blogArticle is null)
            {
                return this.NotFound();
            }

            var author = await this.employeeService.GetEmployeeAsync(blogArticle.AuthorId);

            if (author is null)
            {
                return this.NotFound();
            }

            return this.Ok(new BlogArticleFullInfo(blogArticle, author));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogArticleAsync([FromBody] BlogArticle blogArticle)
        {
            if (blogArticle is null)
            {
                throw new ArgumentNullException(nameof(blogArticle));
            }

            var author = await this.employeeService.GetEmployeeAsync(blogArticle.AuthorId);
            if (author is null)
            {
                return this.NotFound();
            }

            await this.bloggingService.CreateBlogArticleAsync(blogArticle);
            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogArticleAsync(int id)
        {
            if (!await this.bloggingService.DeleteBlogArticleAsync(id))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlogArticleAsync(int id, [FromBody] BlogArticle blogArticle)
        {
            if (!await this.bloggingService.UpdateBlogArticleAsync(id, blogArticle))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }
    }
}
