using HeAndSheStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace HeAndSheStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(int id)
        {
            
            var slider = _context.Sliders.ToList();
            var service = _context.Services.ToList();
            var category = _context.Categories.ToList();
            var videoservice = _context.Videoservices.FirstOrDefault();
            var aboutus = _context.Aboutus.ToList();
            var product = _context.Products.OrderByDescending(x => x.Productid).Take(4).ToList();
            var testimonial = _context.Testimonials.Include(z => z.User).Where(x => x.Isapproved == true && x.Userid != null).OrderByDescending(x => x.TestimonialId).Take(3).ToList(); 
            var models = Tuple.Create<IEnumerable<Slider>, IEnumerable<Service>, IEnumerable<Category>, Videoservice, IEnumerable<Aboutu>, IEnumerable<Product>, IEnumerable<Testimonial>>(slider, service, category, videoservice, aboutus, product, testimonial);
            return View(models);
        }
     

        public IActionResult About()
        {
            var team = _context.Ourteams.ToList();
            var service = _context.Services.ToList();
            var aboutus = _context.Aboutus.ToList();
            var videoservice = _context.Videoservices.FirstOrDefault();
            var models = Tuple.Create<IEnumerable<Ourteam>, IEnumerable<Service>, Videoservice, IEnumerable<Aboutu>>(team, service, videoservice, aboutus);
            return View(models);
        }
        public IActionResult Product(int id , string? searchString)
        {
            var product = _context.Products.ToList();
            if (searchString  != null)
            {
                 product = _context.Products.Where(p => p.Productname.ToLower().Contains(searchString.ToLower())).ToList();
            }
        
            var category = _context.Categories.ToList();
            var prod = _context.Products.Where(p => p.Productid == id).SingleOrDefault();
            var topRatedProducts = _context.Reviews.Where(review => review.Rating == 5)
                                .OrderByDescending(review => review.Reviewdate)
                                .Take(3)
                                .ToList();
            var models = Tuple.Create<IEnumerable<Product>, IEnumerable<Category>, Product ,IEnumerable<Review>>(product, category, prod , topRatedProducts);
            return View(models);
        }

       
        public IActionResult ProductDetails(int id)
        {
            var product = _context.Products.Where(p => p.Productid == id).Include(x => x.Category).SingleOrDefault();
            var review = _context.Reviews.Include(c => c.User).Where(r => r.Productid == id).ToList();
            var reviewTwo = _context.Reviews.Include(r => r.Product).Include(r => r.User).FirstOrDefault(m => m.Reviewid == id);
            ViewBag.reviewNumber = review.Count();
            var models = Tuple.Create<Product, IEnumerable<Review> , Review >(product, review , reviewTwo);
            var averageRating = _context.Reviews
                .Where(x => x.Productid == id)
                .Select(x => (double?)x.Rating)
                .Average();

            ViewBag.rating = averageRating ?? 0;
            ViewBag.NumberOfRating = _context.Reviews
                .Where(x => x.Productid == id).Count();


            return View(models);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(decimal? productId, decimal? quantity)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Register", "LoginRegister");
            }

            if (productId != null && quantity != null)
            {
                var existingCartItem = _context.Carts.FirstOrDefault(c => c.Productid == productId && c.Userid == userId);

                if (existingCartItem != null)
                {

                    existingCartItem.Quantity += quantity;

                }
                else
                {

                    var product = _context.Products.FirstOrDefault(p => p.Productid == productId);

                    if (product != null)
                    {
                        decimal? totalPrice = product.Price * quantity;

                        var cart = new Cart
                        {
                            Userid = userId.Value,
                            Productid = productId,
                            Cardid = null,
                            Quantity = quantity,
                            Orderstatus = "Pending",
                            Totalprice = totalPrice
                        };

                        _context.Carts.Add(cart);
                    }
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction("ProductDetails", new { id = productId });
        }

        public async Task<IActionResult> DeleteItem(decimal? id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");


            if (_context.Carts == null)
            {
                return Problem("Entity set 'ModelContext.Carts'  is null.");
            }

            var cart = _context.Carts.Where(p => p.Productid == id && p.Userid == userId).SingleOrDefault();
            if (cart != null)
            {
                _context.Carts.Remove(cart);

            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Cart", "Home");
        }
        public async Task<IActionResult> Cart()

        {
            int? id = HttpContext.Session.GetInt32("UserId");

            if (id == null)
            {
                return RedirectToAction("Register", "LoginRegister");
            }


            var cart = await _context.Carts.Include(c => c.Product).Where(x => x.Userid == id).ToListAsync();
            if (cart == null)
            {

                return RedirectToAction("Cart", "Home");
            }

            decimal? totalPrice = cart.Sum(item => item.Quantity * item.Product.Price);
            ViewData["TotalPrice"] = totalPrice;
            ViewData["SubTotal"] = totalPrice + 3;
            ViewData["CartCount"] = cart.Count();

            return View(cart);


        }
        [HttpGet]
        public IActionResult Checkout()
        {
            int? id = HttpContext.Session.GetInt32("UserId");

            if (id == null)
            {
                return RedirectToAction("Register", "LoginRegister");
            }


            var cart = _context.Carts.Include(c => c.Product).Where(c => c.Userid == id).ToList();
            decimal? subTotal = cart.Sum(item => item.Quantity * item.Product.Price);
            decimal? totalPrice = subTotal + 3;
            ViewData["TotalPrice"] = totalPrice;
            ViewData["SubTotal"] = subTotal;
            ViewData["CartCount"] = cart.Count();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCheckout([Bind("Id,Address,Email,Country,Zip,Fname,Lname,Message,Userid")] Billing billing, string cardHolder, int cardNumber, string cvv, DateTime date)
        {
            if (ModelState.IsValid)
            {
                int? id = HttpContext.Session.GetInt32("UserId");

                if (id == null)
                {
                    return RedirectToAction("Register", "LoginRegister");
                }

                var cart = await _context.Carts.Include(c => c.Product).Where(c => c.Userid == id).ToListAsync();


                decimal? subTotal = cart.Sum(item => item.Quantity * item.Product.Price);
                decimal? totalPrice = subTotal + 3;
                ViewData["TotalPrice"] = totalPrice;


                var pay = _context.Payments.Where(x => x.Cardholdername == cardHolder && x.Cardnumber == cardNumber && x.Expirationdate.Value.Date == date.Date && x.Cvv == cvv).SingleOrDefault();

                if (pay == null)
                {
                    return Json(new { success = false, message = "Card Details Are Incorrect" });
                }
                if (pay.Balance >= totalPrice)
                {
                    pay.Balance -= totalPrice;
                    _context.Payments.Update(pay);
                    await _context.SaveChangesAsync();
                    billing.Userid = id;
                    _context.Billings.Add(billing);


                    var order = new Orderr
                    {
                        Orderdate = DateTime.Now,
                        Totalamount = totalPrice,
                        Status = "Pending",
                        Billing = billing,
                        Userid = id
                    };
                    _context.Orderrs.Add(order);

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

                   

                    foreach (var cartItem in cart)
                    {
                        var orderItem = new Itorder
                        {
                            Quantity = cartItem.Quantity,
                            Itemprice = cartItem.Product.Price,
                            Productid = cartItem.Product.Productid,
                            Order = order
                        };
                        _context.Itorders.Add(orderItem);
                    }
                    _context.Carts.RemoveRange(cart);
                    await _context.SaveChangesAsync();


                 


                    return Json(new { success = true, message = "Order placed successfully!" });

                }




                return View();

            }



            return RedirectToAction("Cart", "Home");
        }


       
        [HttpGet]
        public IActionResult CreateTestimonialView()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTestimonial([Bind("TestimonialId,Comments")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                var id = HttpContext.Session.GetInt32("UserId");

                Testimonial testimonial1 = new Testimonial()
                {
                    Comments = testimonial.Comments,
                    Createdate = DateTime.Now,
                    Isapproved = false,
                    Userid = id,
                    Status="New Comment"
                   
                };
                _context.Add(testimonial1);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");

        }


        public IActionResult ContactUs(string name, string email, string subject, string message)
        {
            Contactu contact = new Contactu();
            contact.Message = message;
            contact.Name = name;
            contact.Email = email;
            contact.Subject = subject;

            _context.Contactus.Add(contact);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "The Message Was Sent Successfully";

            MimeMessage message_x = new MimeMessage();
            MailboxAddress from = new MailboxAddress(contact.Name, contact.Email);
            message_x.From.Add(from);
            MailboxAddress to = new MailboxAddress("She_He_Shop", "fariha.yacoub@gmail.com");
            message_x.To.Add(to);
            message_x.Subject = "Arrival of the request";
            BodyBuilder builder = new BodyBuilder();
            builder.HtmlBody = $"<h3>Thank you!</h3>" +
                             $"<p>Name: {contact.Name}</p>" +
                             $"<p>Subject: {contact.Subject}</p>" +
                             $"<p>User Email: {contact.Email}</p>" +
                             $"<p>Message: {contact.Message}</p>";
            message_x.Body = builder.ToMessageBody();

            using (var clinte = new SmtpClient())
            {
                clinte.Connect("smtp.gmail.com", 465, true);
                clinte.Authenticate("fariha.yacoub@gmail.com", "qgfazwewnsymtpyg");
                clinte.Send(message_x);
                clinte.Disconnect(true);
            }

            return RedirectToAction("Index", "Home");
        }


        public IActionResult Rating(string comment, decimal id, decimal star )
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                Review review = new Review();
                review.Comments = comment;
                review.Rating = star;
                review.Reviewdate = DateTime.Now;
                review.Productid = id;
                review.Userid = userId;
                _context.Add(review);
                _context.SaveChangesAsync();
            }
            
            return RedirectToAction("ProductDetails", "Home", new { id = id });
        }

      
        public IActionResult EditProfile()
        {

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                var user = _context.Userrs.Where(x => x.Userid == userId).SingleOrDefault();
                if (user != null)
                {
                    var favs = _context.Favs.Where(x => x.Userid == userId).Include(x => x.Product).ToList();
                    var login = _context.UserLogins.Where(x => x.Userid == user.Userid).SingleOrDefault();
                    ViewBag.login = login;
                    ViewBag.user = user;
                    ViewBag.fav = favs;

                    // Retrieve user's orders
                    var orders = _context.Orderrs
                        .Where(x => x.Userid == userId)
                        .Include(c => c.Billing)
                        .OrderByDescending(x => x.Orderdate)
                        .ToList();

                    ViewBag.orders = orders;

                    // Filter orders based on status
                    var arrived = orders.Where(x => x.Status == Status.btn4.ToString()).ToList();
                    var notArrived = orders.Where(x => x.Status != Status.btn4.ToString()).ToList();

                    ViewBag.arrived = arrived;
                    ViewBag.notArrived = notArrived;
                    ViewBag.user = user;
                    ViewBag.orders = orders;
                    //var models = Tuple.Create<Userr, IEnumerable<Orderr>>(user, );

                    return View(user);
                }

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile([Bind("Userid,Fname,Lname,Gender,ImageFile")] Userr userr)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            //if (userId != null)
            //{

                if (ModelState.IsValid)
                {


                    //var user = _context.Userrs.Where(x => x.Userid == userId).SingleOrDefault();
                    //if (user != null)
                    //{
                        //var login = _context.UserLogins.Where(x => x.Userid == user.Userid).SingleOrDefault();
                        //ViewBag.image = user.Imagepath;
                        if (userr.ImageFile != null)
                        {
                            string wwwRootPath = _webHostEnvironment.WebRootPath;
                            string fileName = Guid.NewGuid().ToString() + "_" + userr.ImageFile.FileName;
                            string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                            using (var filestram = new FileStream(path, FileMode.Create))
                            {
                                await userr.ImageFile.CopyToAsync(filestram);
                            }
                            userr.Imagepath = fileName;
                          
                        }
                        
                        else
                        {
                            userr.Imagepath = _context.Userrs.Where(x => x.Userid == userId).AsNoTracking<Userr>().SingleOrDefault().Imagepath;
                        }
                       
                   
                        //user.Fname = userr.Fname;
                        //user.Lname = userr.Lname;
                        //user.Imagepath = userr.Imagepath;

                        _context.Update(userr);
                        _context.SaveChangesAsync();
                        //if (login != null)
                        //{
                        //    login.Email = Email;
                        //    _context.Update(login);
                        //     _context.SaveChangesAsync();
                        //}
                        
                        
                        return RedirectToAction("EditProfile", "Home");
                    }


                    return RedirectToAction("EditProfile", "Home");
                //}
            //}



           
        }

     
        private bool UserrExists(decimal id)
        {
            return (_context.Userrs?.Any(e => e.Userid == id)).GetValueOrDefault();
        }
        public IActionResult DetailsOrder(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                var itOrder = _context.Itorders.Where(x => x.Orderid == id).Include(c => c.Product).ToList();
                var order = _context.Orderrs.Where(c => c.Orderid == id).SingleOrDefault();
                if (order != null)
                {
                    ViewBag.order = order;
                    ViewBag.total = itOrder.Sum(c => c.Itemprice * c.Quantity);

                    return View(itOrder);

                }
                return RedirectToAction("Index", "Home");


            }

            return RedirectToAction("Index", "Home");


        }

        public IActionResult GetProductsByCategory( int id)
        {
            var products = _context.Products.Where(p => p.CategoryId == id).ToList();
            var userId = HttpContext.Session.GetInt32("UserId");

            var averageRatings = _context.Products
             .Where(p => p.CategoryId == id)
             .GroupJoin(
                 _context.Reviews,
                 product => product.Productid,
                 review => review.Productid,
                 (product, reviews) => new
                 {
                     ProductId = product.Productid,
                     Average = (double?)(reviews.Any() ? reviews.Average(x => x.Rating) : 0)
                 }
             )
             .ToList();


            ViewBag.catId = id;
            ViewBag.averageRatings = averageRatings;

            return View(products);
        }

        [HttpPost]
        public IActionResult GetProductsByCategory(int Id, string searchText, string? store)
        {
            var products = _context.Products.Where(p => p.CategoryId == Id).ToList();

            var averageRatings = _context.Products
            .Where(p => p.CategoryId == Id)
            .GroupJoin(
                _context.Reviews,
                product => product.Productid,
                review => review.Productid,
                (product, reviews) => new
                {
                    ProductId = product.Productid,
                    Average = (double?)(reviews.Any() ? reviews.Average(x => x.Rating) : 0)
                }
            )
            .ToList();

            if (searchText != null)
            {
                products = _context.Products
                  .Where(p => p.CategoryId == Id && p.Productname.ToLower().Contains(searchText.ToLower()))
                  .ToList();

            }

            if (store == "D")
            {
                products = _context.Products.Where(p => p.CategoryId == Id).ToList();
            }
            else if (store == "ETS")
            {
                products = _context.Products.Where(p => p.CategoryId == Id).OrderByDescending(x => x.Price - (x.Price * x.Sale / 100)).ToList();
            }
            else if (store == "STE")
            {
                products = _context.Products.Where(p => p.CategoryId == Id).OrderBy(x => x.Price - (x.Price * x.Sale / 100)).ToList();
            }
            else if (store == "NTO")
            {
                products = _context.Products.Where(p => p.CategoryId == Id).OrderByDescending(x => x.Productid).ToList();
            }
            else if (store == "OTN")
            {
                products = _context.Products.Where(p => p.CategoryId == Id).OrderBy(x => x.Productid).ToList();
            }

            else if (store == "LR")
            {

                var x = _context.Products
                      .Where(p => p.CategoryId == Id)
                      .GroupJoin(
                          _context.Reviews,
                          product => product.Productid,
                          review => review.Productid,
                          (product, reviews) => new
                          {
                              Productid = product.Productid,
                              Average = reviews.Any() ? (double?)reviews.Average(x => x.Rating) : null
                          }
                      ).ToList();

                var y = x.OrderBy(x => x.Average).ToList();
                var filteredProducts = products
               .Where(p => y.Any(item => item.Productid == p.Productid))
               .OrderBy(p => y.FindIndex(item => item.Productid == p.Productid))
               .ToList();
                products = filteredProducts;

            }

            else if (store == "HR")
            {
                var x = _context.Products
                                  .Where(p => p.CategoryId == Id)
                                  .GroupJoin(
                                      _context.Reviews,
                                      product => product.Productid,
                                      review => review.Productid,
                                      (product, reviews) => new
                                      {
                                          Productid = product.Productid,
                                          Average = reviews.Any() ? (double?)reviews.Average(x => x.Rating) : null
                                      }
                                  ).ToList();

                var y = x.OrderByDescending(x => x.Average).ToList();
                var filteredProducts = products
               .Where(p => y.Any(item => item.Productid == p.Productid))
               .OrderBy(p => y.FindIndex(item => item.Productid == p.Productid))
               .ToList();
                products = filteredProducts;
            }

          
            ViewBag.catId = Id;




            ViewBag.averageRatings = averageRatings;
            return View(products);
        }

        public IActionResult Search(int Id)
        {
            var products = _context.Products.Where(p => p.CategoryId == Id).ToList();
            var userId = HttpContext.Session.GetInt32("UserId");
            var averageRatings = _context.Products
             .Where(p => p.CategoryId == Id)
             .GroupJoin(
                 _context.Reviews,
                 product => product.Productid,
                 review => review.Productid,
                 (product, reviews) => new
                 {
                     ProductId = product.Productid,
                     Average = (double?)(reviews.Any() ? reviews.Average(x => x.Rating) : 0)
                 }
             )
             .ToList();


            ViewBag.catId = Id;
           

            return View(products);
        }

        [HttpPost]
        public IActionResult Search(int Id, string searchText, string? store)
        {
            var products = _context.Products.Where(p => p.CategoryId == Id).ToList();

            var averageRatings = _context.Products
            .Where(p => p.CategoryId == Id)
            .GroupJoin(
                _context.Reviews,
                product => product.Productid,
                review => review.Productid,
                (product, reviews) => new
                {
                    ProductId = product.Productid,
                    Average = (double?)(reviews.Any() ? reviews.Average(x => x.Rating) : 0)
                }
            )
            .ToList();

            if (searchText != null)
            {
                products = _context.Products
                  .Where(p => p.CategoryId == Id && p.Productname.ToLower().Contains(searchText.ToLower()))
                  .ToList();

            }

            if (store == "D")
            {
                products = _context.Products.Where(p => p.CategoryId == Id).ToList();
            }
            else if (store == "ETS")
            {
                products = _context.Products.Where(p => p.CategoryId == Id).OrderByDescending(x => x.Price - (x.Price * x.Sale / 100)).ToList();
            }
            else if (store == "STE")
            {
                products = _context.Products.Where(p => p.CategoryId == Id).OrderBy(x => x.Price - (x.Price * x.Sale / 100)).ToList();
            }
            else if (store == "NTO")
            {
                products = _context.Products.Where(p => p.CategoryId == Id).OrderByDescending(x => x.Productid).ToList();
            }
            else if (store == "OTN")
            {
                products = _context.Products.Where(p => p.CategoryId == Id).OrderBy(x => x.Productid).ToList();
            }

            else if (store == "LR")
            {

                var x = _context.Products
                      .Where(p => p.CategoryId == Id)
                      .GroupJoin(
                          _context.Reviews,
                          product => product.Productid,
                          review => review.Productid,
                          (product, reviews) => new
                          {
                              ProductId = product.Productid,
                              Average = reviews.Any() ? (double?)reviews.Average(x => x.Rating) : null
                          }
                      ).ToList();

                var y = x.OrderBy(x => x.Average).ToList();
                var filteredProducts = products
               .Where(p => y.Any(item => item.ProductId == p.Productid))
               .OrderBy(p => y.FindIndex(item => item.ProductId == p.Productid))
               .ToList();
                products = filteredProducts;

            }

            else if (store == "HR")
            {
                var x = _context.Products
                                  .Where(p => p.CategoryId == Id)
                                  .GroupJoin(
                                      _context.Reviews,
                                      product => product.Productid,
                                      review => review.Productid,
                                      (product, reviews) => new
                                      {
                                          ProductId = product.Productid,
                                          Average = reviews.Any() ? (double?)reviews.Average(x => x.Rating) : null
                                      }
                                  ).ToList();

                var y = x.OrderByDescending(x => x.Average).ToList();
                var filteredProducts = products
               .Where(p => y.Any(item => item.ProductId == p.Productid))
               .OrderBy(p => y.FindIndex(item => item.ProductId == p.Productid))
               .ToList();
                products = filteredProducts;
            }

            
            ViewBag.catId = Id;
            return View(products);
        }

        public IActionResult AddToWishList(int Id, int? categoryId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var product = _context.Products.FirstOrDefault(x => x.Productid == Id);
            var fav = _context.Favs.FirstOrDefault(x => x.Userid == userId && x.Productid == Id);

            if (product != null && fav == null)
            {
                Fav wishList = new Fav();
                wishList.Userid = userId;
                wishList.Productid = Id;
                _context.Favs.Add(wishList);
                _context.SaveChanges();
                TempData["AddToWishList"] = "Success Add " + product.Productname + " To WishList";

            }

            if (product != null && fav != null)
            {
                _context.Favs.Remove(fav);
                _context.SaveChanges();
                TempData["RemoveToWishList"] = "Success Remove " + product.Productname + " To WishList";

            }
            if (categoryId == null)
            {
                return RedirectToAction("ProductDetails", "Home", new { id = Id });

            }
            return RedirectToAction("GetProductsByCategory", "Home", new { id = categoryId });
        }
     
        public IActionResult DeletProductFromWichList(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {

                var product = _context.Favs.Where(x => x.Userid == userId && x.Productid == id).SingleOrDefault();
                if (product != null)
                {
                    _context.Favs.Remove(product);
                    _context.SaveChanges();
                }

                return RedirectToAction("EditProfile", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}