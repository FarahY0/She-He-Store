using HeAndSheStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HeAndSheStore.Controllers
{
    public class LoginRegisterController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LoginRegisterController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LogIn([Bind("UserName,Password")] UserLogin userLogin)
        {
            var auth = _context.UserLogins.Where(
                x=>x.UserName == userLogin.UserName && x.Password == userLogin.Password
                ).SingleOrDefault();

            if (auth != null)
            {

                HttpContext.Session.SetInt32("UserId", (int)auth.Userid);
                HttpContext.Session.SetString("UserName", auth.UserName);
                

                switch (auth.Roleid)
                {
                    case 1: HttpContext.Session.SetString("AdminName",auth.UserName);
                        
                        return RedirectToAction("Dashboard","Admin");

                    case 2: // Regular User
                        return RedirectToAction("Index", "Home");
                        //case 2:
                        //    HttpContext.Session.SetInt32("UserId",(int)auth.Userid);
                        //    return RedirectToAction("Index", "Home");
                        //case 3:
                        //    HttpContext.Session.SetInt32("UserName", auth.UserName);
                        //    return RedirectToAction("Index", "Home");

                }
            }
            //var user=_context.Userrs.Where(x=>x.Userid == auth.Userid).SingleOrDefault();
            ViewBag.userName = userLogin.UserName;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Userid,Fname,Lname")] Userr userr , string userName , string Password )
        {
            if (ModelState.IsValid)
            {
                _context.Add(userr);
                await _context.SaveChangesAsync();
                //add user login details
                UserLogin login = new UserLogin();
                login.Roleid = 2;
                login.UserName = userName;
                login.Password = Password;
                login.Email = userName;
                login.Userid = userr.Userid;

                _context.Add(login);

                await _context.SaveChangesAsync();
                return RedirectToAction("LogIn");
            }
            return View("Register");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }


        public async Task<IActionResult> MainProfile(decimal? id)
        {
            if (id == null || _context.Userrs == null)
            {
                return NotFound();
            }

            var userr = await _context.Userrs
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (userr == null)
            {
                return NotFound();
            }

            return View(userr);
        }



        [HttpGet]
        public async Task<IActionResult> EditUserProfile(decimal? id)
        {
            if (id == null || _context.Userrs == null)
            {
                return NotFound();
            }

            var userr = await _context.Userrs.FindAsync(id);
            if (userr == null)
            {
                return NotFound();
            }
           
            return View(userr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserProfile([Bind("Userid,Fname,Lname,Gender,ImageFile")] Userr userr )
        {
           

            if (ModelState.IsValid)
            {
                try
                {

                    if (userr.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;

                        // Generate a unique filename for the video.
                        string fileName = Guid.NewGuid().ToString() + "_" + userr.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                        // Save the uploaded video file.
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await userr.ImageFile.CopyToAsync(fileStream);
                        }

                        // Store the video file URL in the database.
                        userr.Imagepath = fileName;
                    }
                    _context.Update(userr);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserrExists(userr.Userid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("MainProfile", "LoginRegister", new { id = userr.Userid });
            }
            _context.SaveChanges();
            return View(userr);
        }


       

        [HttpGet]
        public async Task<IActionResult> EditAdminProfile(decimal? id)
        {
            if (id == null || _context.Userrs == null)
            {
                return NotFound();
            }

            var userr = await _context.Userrs.FindAsync(id);
            if (userr == null)
            {
                return NotFound();
            }
            ViewBag.user = userr;
            return View(userr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdminProfile([Bind("Userid,Fname,Lname,Gender,ImageFile")] Userr userr)
        {


            if (ModelState.IsValid)
            {
                try
                {

                    if (userr.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;

                        // Generate a unique filename for the video.
                        string fileName = Guid.NewGuid().ToString() + "_" + userr.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                        // Save the uploaded video file.
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await userr.ImageFile.CopyToAsync(fileStream);
                        }

                        // Store the video file URL in the database.
                        userr.Imagepath = fileName;
                    }
                    _context.Update(userr);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserrExists(userr.Userid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Dashboard", "Admin", new { id = userr.Userid });
            }
            _context.SaveChanges();
            return View(userr);
        }




       


        private bool UserrExists(decimal id)
        {
            return (_context.Userrs?.Any(e => e.Userid == id)).GetValueOrDefault();
        }


    }
}
