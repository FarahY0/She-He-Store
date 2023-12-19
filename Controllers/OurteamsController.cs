using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HeAndSheStore.Models;
using Microsoft.AspNetCore.Hosting;

namespace HeAndSheStore.Controllers
{
    public class OurteamsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OurteamsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        // GET: Ourteams
        public async Task<IActionResult> Index()
        {
              return _context.Ourteams != null ? 
                          View(await _context.Ourteams.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Ourteams'  is null.");
        }

        // GET: Ourteams/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Ourteams == null)
            {
                return NotFound();
            }

            var ourteam = await _context.Ourteams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ourteam == null)
            {
                return NotFound();
            }

            return View(ourteam);
        }

        // GET: Ourteams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ourteams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageFile,Empname,Empposition,Emplinkedin,Empgmail,Empinstagram")] Ourteam ourteam)
        {
            if (ModelState.IsValid)
            {
                if (ourteam.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;

                    // Generate a unique filename for the video.
                    string fileName = Guid.NewGuid().ToString() + "_" + ourteam.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                    // Save the uploaded video file.
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await ourteam.ImageFile.CopyToAsync(fileStream);
                    }

                    // Store the video file URL in the database.
                    ourteam.Imagepath = fileName;
                }
                _context.Add(ourteam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ourteam);
        }

        // GET: Ourteams/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Ourteams == null)
            {
                return NotFound();
            }

            var ourteam = await _context.Ourteams.FindAsync(id);
            if (ourteam == null)
            {
                return NotFound();
            }
            return View(ourteam);
        }

        // POST: Ourteams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,ImageFile,Empname,Empposition,Emplinkedin,Empgmail,Empinstagram")] Ourteam ourteam)
        {
            if (id != ourteam.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ourteam.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;

                        // Generate a unique filename for the video.
                        string fileName = Guid.NewGuid().ToString() + "_" + ourteam.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                        // Save the uploaded video file.
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await ourteam.ImageFile.CopyToAsync(fileStream);
                        }

                        // Store the video file URL in the database.
                        ourteam.Imagepath = fileName;
                    }
                    _context.Update(ourteam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OurteamExists(ourteam.Id))
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
            return View(ourteam);
        }

        // GET: Ourteams/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Ourteams == null)
            {
                return NotFound();
            }

            var ourteam = await _context.Ourteams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ourteam == null)
            {
                return NotFound();
            }

            return View(ourteam);
        }

        // POST: Ourteams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Ourteams == null)
            {
                return Problem("Entity set 'ModelContext.Ourteams'  is null.");
            }
            var ourteam = await _context.Ourteams.FindAsync(id);
            if (ourteam != null)
            {
                _context.Ourteams.Remove(ourteam);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OurteamExists(decimal id)
        {
          return (_context.Ourteams?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
