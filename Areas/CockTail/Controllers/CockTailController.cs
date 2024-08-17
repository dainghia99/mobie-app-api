using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mobie_app_api.Models;
using CockTailModel = mobie_app_api.Models.CockTail;

namespace mobie_app_api.Areas.CockTail.Controllers
{
    [Area("CockTail")]
    [Route("/cocktail/[action]/{id?}")]
    public class CockTailController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CockTailController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CockTailController(AppDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<CockTailController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        // GET: CockTail/CockTail
        public async Task<IActionResult> Index()
        {
            return View(await _context.CockTails.ToListAsync());
        }

        // GET: CockTail/CockTail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cockTail = await _context.CockTails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cockTail == null)
            {
                return NotFound();
            }

            return View(cockTail);
        }

        // GET: CockTail/CockTail/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CockTail/CockTail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,Description,Image")] CockTailModel cockTail, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                // Xử lý file
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", file.FileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);

                var cockTailCreate = new CockTailModel()
                {
                    Name = cockTail.Name,
                    Price = cockTail.Price,
                    Description = cockTail.Description,
                    Image = "/Images/" + file.FileName
                };

                _context.Add(cockTailCreate);
                file.CopyTo(fileStream);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            return View(cockTail);
        }

        // GET: CockTail/CockTail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cockTail = await _context.CockTails.FindAsync(id);
            if (cockTail == null)
            {
                return NotFound();
            }
            return View(cockTail);
        }

        // POST: CockTail/CockTail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,Image")] CockTailModel cockTail, IFormFile file)
        {
            if (id != cockTail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var cockTailUpdate = await _context.CockTails.FirstOrDefaultAsync(c => c.Id == id);

                    if (cockTailUpdate is null)
                    {
                        return NotFound();
                    }

                    // Xoá đường dẫn file cũ

                    if (!string.IsNullOrEmpty(cockTailUpdate.Image))
                    {
                        var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, cockTailUpdate.Image.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Xoá string file trong models
                    cockTailUpdate.Image = null;

                    // Xử lý file
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", file.FileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);


                    cockTailUpdate.Id = cockTail.Id;
                    cockTailUpdate.Name = cockTail.Name;
                    cockTailUpdate.Price = cockTail.Price;
                    cockTailUpdate.Description = cockTail.Description;
                    cockTailUpdate.Image = "/Images/" + file.FileName;

                    _context.Update(cockTailUpdate);
                    file.CopyTo(fileStream);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CockTailExists(cockTail.Id))
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
            return View(cockTail);
        }

        // GET: CockTail/CockTail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cockTail = await _context.CockTails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cockTail == null)
            {
                return NotFound();
            }

            return View(cockTail);
        }

        // POST: CockTail/CockTail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cockTail = await _context.CockTails.FindAsync(id);
            if (cockTail != null)
            {
                _context.CockTails.Remove(cockTail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CockTailExists(int id)
        {
            return _context.CockTails.Any(e => e.Id == id);
        }
    }
}
