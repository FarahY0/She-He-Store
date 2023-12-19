using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HeAndSheStore.Models;
using System.Net.Mail;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MailKit.Search;

namespace HeAndSheStore.Controllers
{
    public class OrderrsController : Controller
    {
        private readonly ModelContext _context;

        public OrderrsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Orderrs
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Orderrs.Include(o => o.Billing).Include(o => o.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Orderrs/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Orderrs == null)
            {
                return NotFound();
            }

            var orderr = await _context.Orderrs
                .Include(o => o.Billing)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Orderid == id);
            if (orderr == null)
            {
                return NotFound();
            }

            return View(orderr);
        }

        // GET: Orderrs/Create
        public IActionResult Create()
        {
            ViewData["Billingid"] = new SelectList(_context.Billings, "Id", "Id");
            ViewData["Userid"] = new SelectList(_context.Userrs, "Userid", "Userid");
            return View();
        }

        // POST: Orderrs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Orderid,Orderdate,Totalamount,Status,Billingid,Userid")] Orderr orderr)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderr);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Billingid"] = new SelectList(_context.Billings, "Id", "Id", orderr.Billingid);
            ViewData["Userid"] = new SelectList(_context.Userrs, "Userid", "Userid", orderr.Userid);
            return View(orderr);
        }

        // GET: Orderrs/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
       

            if (id == null || _context.Orderrs == null)
            {
                return NotFound();
            }


            var order = await _context.Orderrs
                .Include(c => c.Billing)
                .Include(x => x.Itorders)
                .FirstOrDefaultAsync(x => x.Orderid == id);

            var orderItems = _context.Itorders.Include(p => p.Product).Where(x => x.Orderid == id).ToList();

            if (order == null)
            {
                return NotFound();
            }
            var user = _context.Userrs.Where(x => x.Userid == order.Userid).SingleOrDefault();
            if (user != null)
            {
                ViewBag.name = user.Fname + " " + user.Lname;
                ViewBag.orderItems = orderItems;
            }

            ViewData["Billingid"] = new SelectList(_context.Billings, "Id", "Id", order.Billingid);
            ViewData["Userid"] = new SelectList(_context.Userrs, "Userid", "Userid", order.Userid);
            return View(order);



        }

        // POST: Orderrs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id , string status)
        {
            

            var order = _context.Orderrs.Where(x => x.Orderid == id).Include(c => c.Billing).SingleOrDefault();
            if (order == null) { return NotFound(); }
            order.Status = status;
            _context.Update(order);
            await _context.SaveChangesAsync();
            var user = _context.UserLogins.Where(x => x.Userid == order.Userid).Include(c => c.User).SingleOrDefault();
            MimeMessage message = new MimeMessage();
            MailboxAddress from = new MailboxAddress("She_He_Shop", "fariha.yacoub@gmail.com");
            message.From.Add(from);
            MailboxAddress to = new MailboxAddress(user.User.Fname, user.Email);
            message.To.Add(to);
            message.Subject = "Arrival of the request";
            BodyBuilder builder = new BodyBuilder();
            builder.HtmlBody = "<h3>Thank you!</h3>" + " <p>Your order has been successfully received,Thank you.<p>" + "<h5>Best Regards<h5>";
            message.Body = builder.ToMessageBody();

            using (var clinte = new SmtpClient())
            {
                clinte.Connect("smtp.gmail.com", 465, true);
                clinte.Authenticate("fariha.yacoub@gmail.com", "qgfazwewnsymtpyg");
                clinte.Send(message);
                clinte.Disconnect(true);
            }


            return RedirectToAction("Edit", new { id = id });

        }

        // GET: Orderrs/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Orderrs == null)
            {
                return NotFound();
            }

            var orderr = await _context.Orderrs
                .Include(o => o.Billing)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Orderid == id);
            if (orderr == null)
            {
                return NotFound();
            }

            return View(orderr);
        }

        // POST: Orderrs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Orderrs == null)
            {
                return Problem("Entity set 'ModelContext.Orderrs'  is null.");
            }
            var orderr = await _context.Orderrs.FindAsync(id);
            if (orderr != null)
            {
                _context.Orderrs.Remove(orderr);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderrExists(decimal id)
        {
          return (_context.Orderrs?.Any(e => e.Orderid == id)).GetValueOrDefault();
        }

     







    }
}
