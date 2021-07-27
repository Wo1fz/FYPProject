using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYPProject.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using AutoMapper;
using System.Net;

namespace FYPProject.Controllers
{
    [Authorize(Roles = "Moderator")]
    public class ModeratorController : CommonController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Moderator
        public ActionResult Index()
        {
            var campaigns = db.Campaigns.Where(x => x.Title != null).ToList();
            return View(campaigns);
        }

        public ActionResult ViewReports(int id)
        {
            var reports = db.Report.Where(x => x.CampaignId == id).ToList();
            return View(reports);
        }
    }
}