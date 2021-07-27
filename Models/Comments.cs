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
    public class Comments
    {
        [Key]
        public int CommentId { get; set; }
        public int CampaignId { get; set; }
        public string Text { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? DeletionDate { get; set; }

        [Required]
        [ForeignKey("User")]
        public string OwnerId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public string OwnerFullname { get; set; }

        private ApplicationDbContext context = new ApplicationDbContext();

        public int PostComment(CommentViewModel model)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = userManager.FindById(model.OwnerId);

            if (user != null)
            {

                Campaign campaign = context.Campaigns.Find(model.CampaignId);

                if (campaign != null)
                {
                    Comments comment = new Comments()
                    {
                        CampaignId = model.CampaignId,
                        Text = model.Text,
                        CreationDate = DateTime.Now,
                        OwnerId = user.Id,
                        OwnerFullname = user.FullName
                    };

                    context.Comments.Add(comment);
                    context.SaveChanges();
                }

                return model.CampaignId;
            }
            return 0;
        }
    }
}