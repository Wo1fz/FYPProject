using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FYPProject.Models
{
    public class Follows
    {
        [Key]
        public int FollowId { get; set; }
        public int CampaignId { get; set; }

        [Required]
        [ForeignKey("User")]
        public string Id { get; set; }
        public virtual User User { get; set; }
        [NotMapped]
        public List<Campaign> campaigns { get; set; }


        private ApplicationDbContext context = new ApplicationDbContext();
        public int FollowCampaign(FollowViewModel model)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = userManager.FindById(model.Id);

            if (user != null)
            {

                Campaign campaign = context.Campaigns.Find(model.CampaignId);

                if (campaign != null)
                {
                    Follows follow = new Follows()
                    {
                        CampaignId = model.CampaignId,
                        Id = user.Id,
                    };

                    context.Follow.Add(follow);
                    campaign.Followers += 1;
                    context.SaveChanges();
                }

                return model.CampaignId;
            }
            return 0;
        }

        public int UnfollowCampaign(FollowViewModel model)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = userManager.FindById(model.Id);

            if (user != null)
            {
                Campaign campaign = context.Campaigns.Find(model.CampaignId);

                if (campaign != null)
                {
                    var followedRecord = context.Follow.Where(x => x.CampaignId == campaign.CampaignId && x.Id == user.Id).Select(x => x.FollowId).FirstOrDefault();
                    Follows followed = context.Follow.Find(followedRecord);
                    context.Follow.Remove(followed);
                    campaign.Followers -= 1;
                    context.SaveChanges();
                }

                return model.CampaignId;
            }
            return 0;
        }
    }
}