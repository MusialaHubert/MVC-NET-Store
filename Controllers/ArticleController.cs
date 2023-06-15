using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Models;
using Shop.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ArticleController : Controller
    {
        private readonly ShopDbContext dbContext;
        private readonly IWebHostEnvironment environment;

        public ArticleController(ShopDbContext context, IWebHostEnvironment environment)
        {
            dbContext = context;
            this.environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            Console.WriteLine(dbContext.Articles.ToList());

            var shopDbContext = dbContext.Articles.Include(a => a.Category);
            return View(await shopDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var article = await dbContext.Articles.
                Include(s => s.Category)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article is null)
            {
                return NotFound();
            }

            return View(article);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(dbContext.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name, Price, Photo, CategoryId")] CreateArticleModel article)
        {
            var newArticle = new Article()
            {
                Id = article.Id,
                Name = article.Name,
                Price = article.Price,
                Photo = "",
                CategoryId = article.CategoryId,
            };

            if (ModelState.IsValid)
            {              
                if (article.Photo is not null)
                {
                    string rootPath = environment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(article.Photo.FileName);
                    string extension = Path.GetExtension(article.Photo.FileName);
                    newArticle.Photo = fileName = fileName + DateTime.Now.ToString("yymmssffff") + extension;
                    string path = Path.Combine(rootPath + "/upload", fileName);
                    using(var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await article.Photo.CopyToAsync(fileStream);
                    }
                }                               
               
                dbContext.Add(newArticle);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(dbContext.Categories, "Id", "Name", article.CategoryId);
            return View(article);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var article = await dbContext.Articles.
                Include(s => s.Category)
                .FirstOrDefaultAsync(a => a.Id == id);

            Console.WriteLine(article.Category);
            if (article is null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(dbContext.Categories, "Id", "Name", article.CategoryId);
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Price, Photo, CategoryId")] Article articleNew)
        {
            if (id != articleNew.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var article = await dbContext.Articles.FirstOrDefaultAsync(c => c.Id == id);
                if (article is null)
                {
                    return NotFound();
                }

                article.Name = articleNew.Name;
                article.Price = articleNew.Price;
                article.CategoryId = articleNew.CategoryId;
                dbContext.Update(article);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var article = await dbContext.Articles
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article is null)
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await dbContext.Articles.FirstOrDefaultAsync(s => s.Id == id);
         
            if(article is not null)
            {
                var imagePath = Path.Combine(environment.WebRootPath, "upload", article.Photo);

                if(System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                dbContext.Articles.Remove(article);
                await dbContext.SaveChangesAsync();
            }          
            return RedirectToAction(nameof(Index));
        }
    }
}
