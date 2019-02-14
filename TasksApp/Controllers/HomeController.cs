using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TasksApp.Data;
using TasksApp.Models;

namespace TasksApp.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            UserRepository userRepo = new UserRepository(Properties.Settings.Default.conStr);
            IndexViewModel ivm=new IndexViewModel();
            ivm.UserId = userRepo.getIdByEmail(User.Identity.Name);  
            return View(ivm);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(string FirstName, string LastName, string EmailAddress, string Password)
        {
            UserRepository userRepo = new UserRepository(Properties.Settings.Default.conStr);
            userRepo.signUp(FirstName, LastName, EmailAddress, Password);
            
            return Redirect("/Home/Index");
        }

        public ActionResult Login()
        {
            LoginViewModel lvm = new LoginViewModel();
            if (TempData["failedLogin"] != null)
            {
                lvm.Message = (string)TempData["failedLogin"];
            }
            if (TempData["logOut"] != null)
            {
                lvm.Message = (string)TempData["logOut"];
            }

            return View(lvm);
        }

        [HttpPost]
        public ActionResult SignIn(string EmailAddress, string Password)
        {
            UserRepository userRepo = new UserRepository(Properties.Settings.Default.conStr);
            User user = userRepo.signedInUser(EmailAddress, Password);
            if (user == null)
            {
                TempData["failedLogin"] = "Login Failed-please try again";
                return Redirect("/Home/Login");
            }

            FormsAuthentication.SetAuthCookie(EmailAddress, true);
            
            return Redirect("/Home/Index");
        }

        public ActionResult Logout()
        {
            TempData["logOut"] = "You have just logged out. Please log in again to access the site";
            FormsAuthentication.SignOut();
            return Redirect("/Home/Login");
        }

        
       
        
    }
}