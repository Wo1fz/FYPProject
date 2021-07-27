using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYPProject.Models;

namespace FYPProject.Controllers
{
    public class CommonController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void UpdateCampaignStatus()
        {
            var campaigns = db.Campaigns.Where(x => x.Title != null).ToList();
            List<Campaign> tempCampaign = new List<Campaign>();
            foreach (var tempitem in campaigns)
            {
                tempCampaign.Add(tempitem);
            }
            foreach (var tempitem in tempCampaign)
            {
                DateTime endDate = Convert.ToDateTime(tempitem.EndDate);
                int daysremaining = Convert.ToInt32(((Convert.ToDateTime(endDate) - DateTime.Now).TotalDays));

                if (daysremaining <= 0)
                {
                    tempitem.Status = "Closed";
                    db.SaveChanges();
                }                
           }
        }
    }
}