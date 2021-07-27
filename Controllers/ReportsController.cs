using FYPProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FYPProject.Controllers
{
    public class ReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Reports report = new Reports();
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult ReportCampaign(int id)
        {
            Campaign campaign = db.Campaigns.Where(x => x.CampaignId == id).FirstOrDefault();
            ReportViewModel model = new ReportViewModel
            {
                CampaignId = campaign.CampaignId,
                ReporterId = this.User.Identity.GetUserId(),
                Username = this.HttpContext.User.Identity.Name,              
            };
            return View(model);
        }

        // GET: Campaign/CreateCampaign
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportCampaign(ReportViewModel model, int id)
        {
            var username = this.HttpContext.User.Identity.Name;
            Campaign campaign = db.Campaigns.Where(x => x.CampaignId == id).FirstOrDefault();
            var norepeat = db.Report.Where(x => x.Username == username && x.CampaignId == campaign.CampaignId).FirstOrDefault();
            if (norepeat == null)
            {
                if (ModelState.IsValid)
                {
                    var result = report.Report(model);
                    if (result != 0)
                    {
                        return RedirectToAction("ReportView", "Reports");
                    }
                    else
                    {
                        return View(model);
                    }
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("ReportDuplicateView", "Reports");
            }
                       
        }

        [Authorize]
        public ActionResult ReportView()
        {
            return View();
        }

        public ActionResult ReportDuplicateView()
        {
            return View();
        }
    }
}