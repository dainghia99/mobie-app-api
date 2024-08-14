using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mobie_app_api.Models;

namespace mobie_app_api.Areas.Product.Controllers
{
    [Area("Product")]
    [Route("/product/[action]/{id?}")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Product/Product
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cats.ToListAsync());
        }

        // GET: Product/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _context.Cats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cat == null)
            {
                return NotFound();
            }

            return View(cat);
        }

        // GET: Product/Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Image")] Cat cat, IFormFile file)
        {
            if (ModelState.IsValid)
            {

                // Images
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", file.FileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);

                // Create
                var catCreate = new Cat()
                {
                    Name = cat.Name,
                    Description = cat.Description,
                    Image = "/Images/" + file.FileName
                };

                _context.Add(catCreate);
                file.CopyTo(fileStream);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            return View(cat);
        }

        // GET: Product/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _context.Cats.FindAsync(id);
            if (cat == null)
            {
                return NotFound();
            }
            return View(cat);
        }

        // POST: Product/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Image")] Cat cat, IFormFile file)
        {
            if (id != cat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", file.FileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);

                    var catUpdate = new Cat()
                    {
                        Id = cat.Id,
                        Name = cat.Name,
                        Description = cat.Description,
                        Image = "/Images/" + file.FileName
                    };

                    _context.Update(catUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatExists(cat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cat);
        }

        // GET: Product/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _context.Cats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cat == null)
            {
                return NotFound();
            }

            return View(cat);
        }

        // POST: Product/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cat = await _context.Cats.FindAsync(id);
            if (cat != null)
            {
                _context.Cats.Remove(cat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatExists(int id)
        {
            return _context.Cats.Any(e => e.Id == id);
        }

        // API


        [HttpGet]
        [Route("/api/products/")]
        public async Task<IActionResult> GetAll()
        {
            var cats = await _context.Cats.ToArrayAsync();
            return Ok(cats);
        }
    }
}
