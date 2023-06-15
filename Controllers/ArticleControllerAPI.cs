using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [Route("api/article")]
    [ApiController]
    public class ArticleControllerAPI : ControllerBase
    {
        private readonly ShopDbContext dbContext;

        public ArticleControllerAPI(ShopDbContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        public IEnumerable<Article> Get()
        {
            return dbContext.Articles.Include(a => a.Category);
        }

        [HttpGet("{categoryId}/{number}")]
        public IEnumerable<Article> GetProductsFromCategory(int categoryId, int number)
        {
            return dbContext.Articles
                .Include(a => a.Category)
                .Where(a => a.CategoryId == categoryId)
                .OrderBy(a => a.Id)
                .Skip(number)
                .Take(2);
        }

        [HttpGet("{id}")]
        public Article Get(int id)
        {
            return dbContext.Articles
                .Include(a => a.Category)
                .FirstOrDefault(c => c.Id == id);
        }

        [HttpPost]
        public Article Post([FromBody] Article article)
        {
            article.Photo = "";
            dbContext.Articles.Add(article);
            dbContext.SaveChanges();
            return article;
        }

        [HttpPut]
        public Article Put([FromBody] Article article)
        {
            dbContext.Articles.Update(article);
            dbContext.SaveChanges();
            return article;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var article = dbContext.Articles.FirstOrDefault(c => c.Id == id);
            if (article is null) return BadRequest();

            dbContext.Articles.Remove(article);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
