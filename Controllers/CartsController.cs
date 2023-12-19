using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HeAndSheStore.Models;

namespace HeAndSheStore.Controllers
{
    public class CartsController : Controller
    {
        private readonly ModelContext _context;

        public CartsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Carts.Include(c => c.Card).Include(c => c.Product).Include(c => c.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Card)
                .Include(c => c.Product)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Cartid == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["Cardid"] = new SelectList(_context.Payments, "Cardid", "Cardid");
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid");
            ViewData["Userid"] = new SelectList(_context.Userrs, "Userid", "Userid");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cartid,Quantity,Orderstatus,Totalprice,Orderdate,Productid,Userid,Cardid")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Cardid"] = new SelectList(_context.Payments, "Cardid", "Cardid", cart.Cardid);
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid", cart.Productid);
            ViewData["Userid"] = new SelectList(_context.Userrs, "Userid", "Userid", cart.Userid);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["Cardid"] = new SelectList(_context.Payments, "Cardid", "Cardid", cart.Cardid);
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid", cart.Productid);
            ViewData["Userid"] = new SelectList(_context.Userrs, "Userid", "Userid", cart.Userid);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Cartid,Quantity,Orderstatus,Totalprice,Orderdate,Productid,Userid,Cardid")] Cart cart)
        {
            if (id != cart.Cartid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Cartid))
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
            ViewData["Cardid"] = new SelectList(_context.Payments, "Cardid", "Cardid", cart.Cardid);
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid", cart.Productid);
            ViewData["Userid"] = new SelectList(_context.Userrs, "Userid", "Userid", cart.Userid);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Card)
                .Include(c => c.Product)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Cartid == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Carts == null)
            {
                return Problem("Entity set 'ModelContext.Carts'  is null.");
            }
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(decimal id)
        {
          return (_context.Carts?.Any(e => e.Cartid == id)).GetValueOrDefault();
        }
    }
}
