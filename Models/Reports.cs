using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FYPProject.Models
{
    public class Reports
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        [Key]
        public int ReportId { get; set; }
        public int CampaignId { get; set; }
        public string ReporterId { get; set; }
        public string Username { get; set; }
        public string Body { get; set; }
        public string Reason { get; set; }

        public int Report(ReportViewModel model)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = userManager.FindById(model.ReporterId);

            if (user == null)
            {
                return 0;
            }
            Campaign campaign = context.Campaigns.Find(model.CampaignId);
            Reports report = new Reports
            {
                CampaignId = model.CampaignId,
                ReporterId = user.Id,
                Username = user.UserName,
                Body = model.Body,
                Reason = model.Reason
            };

            context.Report.Add(report);
            campaign.Reports += 1;
            context.SaveChanges();

            return campaign.CampaignId;
        }
    }
}