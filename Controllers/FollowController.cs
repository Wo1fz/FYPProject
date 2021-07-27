using FYPProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FYPProject.Controllers
{
    public class FollowController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Follows follow = new Follows();
        // GET: Follow
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FollowCampaign()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FollowCampaign(FollowViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var campaign = db.Campaigns.Find(model.CampaignId);
                if (campaign.CampaignId != 0)
                {
                    return RedirectToAction("ViewCampaign", "Campaign", new { id = campaign.CampaignId });
                }
                return RedirectToAction("ViewCampaign", "Campaign", new { id = model.CampaignId });
            }


            model.Id = User.Identity.GetUserId();
            var result = follow.FollowCampaign(model);
            if (result != 0)
            {
                Campaign campaign = db.Campaigns.Find(result);
                if (campaign.CampaignId == 0)
                {
                    return RedirectToAction("ViewCampaign", "Campaign", new { id = result });
                }
                else
                {
                    return RedirectToAction("ViewCampaign", "Campaign", new { id = campaign.CampaignId });
                }
            }
            return RedirectToAction("ViewCampaign", "Campaign", new { id = model.CampaignId });
        }

        public ActionResult UnfollowCampaign(FollowViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var campaign = db.Campaigns.Find(model.CampaignId);
                if (campaign.CampaignId != 0)
                {
                    return RedirectToAction("ViewCampaign", "Campaign", new { id = campaign.CampaignId });
                }
                return RedirectToAction("ViewCampaign", "Campaign", new { id = model.CampaignId });
            }

            model.Id = User.Identity.GetUserId();
            var result = follow.UnfollowCampaign(model);

            if (result != 0)
            {
                Campaign campaign = db.Campaigns.Find(result);
                if (campaign.CampaignId == 0)
                {
                    return RedirectToAction("ViewCampaign", "Campaign", new { id = result });
                }
                else
                {
                    return RedirectToAction("ViewCampaign", "Campaign", new { id = campaign.CampaignId });
                }
            }
            return RedirectToAction("ViewCampaign", "Campaign", new { id = model.CampaignId });
        }

        public ActionResult FollowedCampaign()
        {
            var userid = User.Identity.GetUserId();
            var followCampaignId = db.Follow.Where(a => a.Id == userid).Select(a => a.CampaignId).ToList();
            var model = db.Campaigns.Where(c => followCampaignId.Contains(c.CampaignId)).ToList();
 
            return View(model);
        }

        [HttpPost]
        public ActionResult CheckFollowed(int id)
        {
            var CurrentUserId = User.Identity.GetUserId();
            bool contactExists = db.Follow.Where(u => u.CampaignId == id && u.Id == CurrentUserId).Any();

            //System.Diagnostics.Debug.WriteLine(contactExists);
            if (contactExists)
            {
                return Json(new { success = true, responseText = "TRUE" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, responseText = "FALSE" }, JsonRequestBehavior.AllowGet);
                
            }
        }
    }

}