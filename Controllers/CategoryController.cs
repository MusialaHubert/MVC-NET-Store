using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ShopDbContext dbContext;

        public CategoryController(ShopDbContext context)
        {
            dbContext = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await dbContext.Categories.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(category is null)
            {
                return NotFound();
            }

            return View(category);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name")]Category category)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(category);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return BadRequest(ModelState);
            
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name")] Category categoryNew)
        {
            if (id != categoryNew.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if(category is null)
                {
                    return NotFound();
                }

                category.Name = categoryNew.Name;
                dbContext.Update(category);
                await dbContext.SaveChangesAsync();             
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
            {
                return NotFound();
            }

            var article = await dbContext.Articles.FirstOrDefaultAsync(c => c.CategoryId == id);

            if(article is not null)
            {
                ViewBag.Error = "Nie mozna usunąc kategorii gdy znajdują się w niej produkty";
                return View(category);
            }


            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
