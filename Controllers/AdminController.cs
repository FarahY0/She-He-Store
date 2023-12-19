using HeAndSheStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;

namespace HeAndSheStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
       

        public AdminController(ModelContext context)
        {
            _context = context;
            
        }
        public IActionResult Dashboard()
        {
            
            ViewData["NumberOfUsers"] = _context.Userrs.Count();
            ViewBag.numberOfOrders = _context.Orderrs.Count();
            ViewBag.numberOfReviews = _context.Reviews.Count();
            ViewBag.numberOfProducts = _context.Products.Count();
            ViewBag.boyNumber = _context.Userrs.Count(x => x.Gender == "Male");
            ViewBag.girlNumber = _context.Userrs.Count(x => x.Gender == "Female");

            var orderCountsByDate = _context.Orderrs
        .ToList() 
        .Where(o => o.Orderdate.HasValue) 
        .GroupBy(o => o.Orderdate.Value.Date) 
        .Select(g => new { Date = g.Key, Count = g.Count() }) 
        .OrderBy(g => g.Date) 
        .ToList();

            ViewBag.orderCountsByDate = orderCountsByDate;

            return View();
        }

      
   




        [HttpGet]
        public IActionResult Report()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                var user = _context.Userrs.ToList();
                var order = _context.Orderrs.ToList();
                var product = _context.Products.ToList();
                var category = _context.Categories.ToList();
                var orderIrem = _context.Itorders.Include(c => c.Order).ToList();

                //Join
                var multiTable = from c in user
                                 join o in orderIrem on c.Userid equals o.Order.Userid
                                 join p in product on o.Productid equals p.Productid
                                 join cat in category on p.CategoryId equals cat.Categoryid
                                 select new HomePage { Product = p, Userr = c, Itorder = o, Category = cat };


                var orderitems = _context.Itorders.Include(p => p.Order).Include(p => p.Product).ToList();


                var allData = Tuple.Create<IEnumerable<HomePage>, IEnumerable<Itorder>>(multiTable, orderitems);
                return View(multiTable);
            }

            return RedirectToAction("Dashboard", "Admin");
        }




        [HttpPost]
        public async Task<IActionResult> Report(DateTime? startDate, DateTime? endDate)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                var user = _context.Userrs.ToList();
                var order = _context.Orderrs.ToList();
                var product = _context.Products.ToList();
                var category = _context.Categories.ToList();
                var Itorder = _context.Itorders.Include(c => c.Order).ToList();

                var multiTable = from u in user
                                 join o in Itorder on u.Userid equals o.Order.Userid
                                 join p in product on o.Productid equals p.Productid
                                 join cat in category on p.CategoryId equals cat.Categoryid
                                 select new HomePage { Product = p, Userr = u, Itorder = o, Category = cat };


                var orderitems = _context.Itorders.Include(p => p.Order).Include(p => p.Product).ToList();
                var modelContext = _context.Itorders.Include(p => p.Order).Include(p => p.Product);

                if (startDate == null && endDate == null)
                {
                    var allData = Tuple.Create<IEnumerable<HomePage>, IEnumerable<Itorder>>(multiTable, modelContext.ToList());

                    return View(multiTable);

                }


                else if (startDate == null && endDate != null)
                {


                    var resultTow = multiTable.Where(x => x.Itorder.Order.Orderdate.Value.Date <= endDate);

                    var result = await modelContext.Where(x => x.Order.Orderdate.Value.Date <= endDate).ToListAsync();
                    var allData = Tuple.Create<IEnumerable<HomePage>, IEnumerable<Itorder>>(resultTow, result);

                    return View(resultTow);
                }
                else if (startDate != null && endDate == null)
                {
                    var resultTow = multiTable.Where(x => x.Itorder.Order.Orderdate.Value.Date >= startDate);

                    var result = await modelContext.Where(x => x.Order.Orderdate.Value.Date >= startDate).ToListAsync();
                    var allData = Tuple.Create<IEnumerable<HomePage>, IEnumerable<Itorder>>(resultTow, result);

                    return View(resultTow);
                }
                else
                {
                    var resultOne = multiTable.Where(x => x.Itorder.Order.Orderdate.Value.Date >= startDate && x.Itorder.Order.Orderdate.Value.Date <= endDate);
                    var result = await modelContext.Where(x => x.Order.Orderdate.Value.Date >= startDate && x.Order.Orderdate.Value.Date <= endDate).ToListAsync();
                    var allData = Tuple.Create<IEnumerable<HomePage>, IEnumerable<Itorder>>(resultOne, result);

                    return View(resultOne);
                }
            }
            return RedirectToAction("Index", "Home");

        }


        public IActionResult GetOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {

                var orders = _context.Orderrs.Where(x => x.Userid == userId).Include(c => c.Billing).OrderByDescending(x => x.Orderdate).ToList();
                var login = _context.UserLogins.Where(x => x.Userid == userId).Include(c => c.User).SingleOrDefault();
                var complete = orders.Where(x => x.Status == Status.btn4.ToString()).ToList();
                var notComplete = orders.Where(x => x.Status != Status.btn4.ToString()).ToList();

                if (login != null)
                {
                    ViewBag.userLogin = login;

                    if (complete != null)
                    {
                        ViewBag.complete = complete;

                    }
                    if (notComplete != null)
                    {
                        ViewBag.notComplete = notComplete;

                    }
                }


                return View(orders);
            }
            return RedirectToAction("Index", "Home");

        }


        public IActionResult GetDetailsOrder(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                var orderItems = _context.Itorders.Where(x => x.Orderid == id).Include(c => c.Product).ToList();

                var order = _context.Orderrs.Where(c => c.Orderid == id).SingleOrDefault();
                if (order != null)
                {
                    ViewBag.order = order;
                    ViewBag.total = orderItems.Sum(c => c.Itemprice * c.Quantity);

                    return View(orderItems);

                }
                return RedirectToAction("Index", "Home");


            }

            return RedirectToAction("Index", "Home");


        }




    }
}
