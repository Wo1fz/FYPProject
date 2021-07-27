using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYPProject.Models;

namespace FYPProject.Controllers
{
    public class HomeController : CommonController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser appuser = new ApplicationUser();
        public ActionResult Index()
        {
            return View();
        }
        public HomeController()
        {
        }
        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SendEmailView()
        {
            return View();
        }

    }
}