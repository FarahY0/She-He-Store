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
    public class VideoservicesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VideoservicesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Videoservices
        public async Task<IActionResult> Index()
        {
              return _context.Videoservices != null ? 
                          View(await _context.Videoservices.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Videoservices'  is null.");
        }

        // GET: Videoservices/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Videoservices == null)
            {
                return NotFound();
            }

            var videoservice = await _context.Videoservices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoservice == null)
            {
                return NotFound();
            }

            return View(videoservice);
        }

        // GET: Videoservices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Videoservices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VideoFile")] Videoservice videoservice)
        {
            if (ModelState.IsValid)
            {
                if (videoservice.VideoFile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;

                    // Generate a unique filename for the video.
                    string fileName = Guid.NewGuid().ToString() + "_" + videoservice.VideoFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Video/", fileName);

                    // Save the uploaded video file.
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await videoservice.VideoFile.CopyToAsync(fileStream);
                    }

                    // Store the video file URL in the database.
                    videoservice.Videourl = fileName;
                }

                _context.Add(videoservice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(videoservice);
        }

        // GET: Videoservices/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Videoservices == null)
            {
                return NotFound();
            }

            var videoservice = await _context.Videoservices.FindAsync(id);
            if (videoservice == null)
            {
                return NotFound();
            }
            return View(videoservice);
        }

        // POST: Videoservices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,VideoFile")] Videoservice videoservice)
        {
            if (id != videoservice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (videoservice.VideoFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;

                        // Generate a unique filename for the video.
                        string fileName = Guid.NewGuid().ToString() + "_" + videoservice.VideoFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Video/", fileName);

                        // Save the uploaded video file.
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await videoservice.VideoFile.CopyToAsync(fileStream);
                        }

                        // Store the video file URL in the database.
                        videoservice.Videourl = fileName;
                    }
                    _context.Update(videoservice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoserviceExists(videoservice.Id))
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
            return View(videoservice);
        }

        // GET: Videoservices/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Videoservices == null)
            {
                return NotFound();
            }

            var videoservice = await _context.Videoservices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoservice == null)
            {
                return NotFound();
            }

            return View(videoservice);
        }

        // POST: Videoservices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Videoservices == null)
            {
                return Problem("Entity set 'ModelContext.Videoservices'  is null.");
            }
            var videoservice = await _context.Videoservices.FindAsync(id);
            if (videoservice != null)
            {
                _context.Videoservices.Remove(videoservice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoserviceExists(decimal id)
        {
          return (_context.Videoservices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
