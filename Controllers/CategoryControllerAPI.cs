using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Database;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryControllerAPI : ControllerBase
    {
        private readonly ShopDbContext dbContext;

        public CategoryControllerAPI(ShopDbContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return dbContext.Categories;
        }

        [HttpGet("{id}")]
        public Category Get(int id)
        {
            return dbContext.Categories.FirstOrDefault(c => c.Id == id);
        }

        [HttpPost]
        public Category Post([FromBody] Category category)
        {
            dbContext.Categories.Add(category);
            dbContext.SaveChanges();
            return category;
        }

        [HttpPut]
        public Category Put([FromBody] Category category)
        {
            dbContext.Categories.Update(category);
            dbContext.SaveChanges();
            return category;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var category = dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null) return BadRequest();           

            var article = dbContext.Articles.FirstOrDefault(c => c.CategoryId == id);
            if (article is not null) return BadRequest("Category is not empty");
           
            dbContext.Categories.Remove(category);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
