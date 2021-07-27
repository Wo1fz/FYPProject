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
    public class Updates
    {
        [Key]
        public int UpdateId { get; set; }
        public int CampaignId { get; set; }
        public string Text { get; set; }
        public DateTime? UpdateDate { get; set; }
        [ForeignKey("User")]
        public string OwnerId { get; set; }
        public virtual User User { get; set; }
        public string OwnerFullname { get; set; }
        private ApplicationDbContext context = new ApplicationDbContext();

        public int PostUpdates(Updates model)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = userManager.FindById(model.OwnerId);

            if (user != null)
            {

                Campaign campaign = context.Campaigns.Find(model.CampaignId);

                if (campaign != null)
                {
                    Updates update = new Updates()
                    {
                        CampaignId = model.CampaignId,
                        Text = model.Text,
                        UpdateDate = DateTime.Now,
                        OwnerId = user.Id,
                        OwnerFullname = user.FullName
                    };

                    context.Update.Add(update);
                    context.SaveChanges();
                }

                return model.CampaignId;
            }
            return 0;
        }
    }

}