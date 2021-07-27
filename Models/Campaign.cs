using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.IO;

namespace FYPProject.Models
{
    public class Campaign
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        public int CampaignId { get; set; }
        public double? DonationGoal { get; set; }
        public double CurrentDonation { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CampaignDuration { get; set; }
        [Required]
        public string OwnerUsername { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Address { get; set; }
        public string Beneficiary { get; set; }
        [Required]
        public string CoverImage { get; set; }
        public string MultipleImages { get; set; }
        [Required]
        public int Donors { get; set; }
        [Required]
        public int Shares { get; set; }
        [Required]
        public int Followers { get; set; }
        [Required]
        public int Reports { get; set; }
        public string PayoutID { get; set; }

        [Required]
        [ForeignKey("User")]
        public string OwnerId { get; set; }
        public virtual User User { get; set; }
        [NotMapped]
        public List<Comments> comments { get; set; }
        [NotMapped]
        public List<Checkout> donors { get; set; }
        [NotMapped]
        public List<Follows> follows { get; set; }
        [NotMapped]
        public List<Updates> updates { get; set; }
        public string Category { get; set; }

        public int Create(CreateCampaignViewModel model)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = userManager.FindById(model.OwnerId);
            string fileName = Path.GetFileNameWithoutExtension(model.CoverImageFile.FileName);
            string extension = Path.GetExtension(model.CoverImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff")+ model.OwnerId + extension;
            model.CoverImage = "~/images/CampaignImages/" + fileName;
            fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/images/CampaignImages/"), fileName);
            model.CoverImageFile.SaveAs(fileName);

            string test = "";

                foreach (var file in model.MultipleImagesFile)
                {
                    if (file == null)
                    {
                        model.MultipleImages = null;
                    }
                    else
                    {
                        string fileName2 = Path.GetFileNameWithoutExtension(file.FileName);
                        string extension2 = Path.GetExtension(file.FileName);
                        fileName2 = fileName2 + DateTime.Now.ToString("yymmssfff") + model.OwnerId + extension2;
                        test = test + "~/images/CampaignImages/" + fileName2 + ",";
                        fileName2 = Path.Combine(HttpContext.Current.Server.MapPath("~/images/CampaignImages/"), fileName2);
                        file.SaveAs(fileName2);
                    }
                    

                }
            model.MultipleImages = test;

            if (user == null)
            {
                return 0;
            }

            Campaign campaign = new Campaign
            {
                DonationGoal = model.DonationGoal,
                CurrentDonation = 0,
                Title = model.Title,
                OwnerId = user.Id,
                Status = "Open",
                OwnerUsername = user.FullName,
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59),
                CampaignDuration = model.CampaignDuration,
                Body = model.Body,
                CoverImage = model.CoverImage,
                MultipleImages = model.MultipleImages,
                PostalCode = model.PostalCode,
                Address = model.Address,
                Beneficiary= model.Beneficiary,
                Donors = 0,
                Followers = 0,
                Reports = 0,
                Shares = 0,
                Category = model.Category
            };
            context.Campaigns.Add(campaign);
            campaign.EndDate = campaign.EndDate.Value.AddDays(campaign.CampaignDuration);
            context.SaveChanges();

            return campaign.CampaignId;
        }
        public List<Campaign> RetrieveCampaigns(string search,string filter)
        {
            return context.Campaigns.Where(x => x.Title.Contains(search) || x.Body.Contains(search)|| search == null).Where(x=>x.Category.Contains(filter) || filter == ""|| search ==null).ToList();
        }

        public DetailsViewModel GetDetails(int id)
        {
            DetailsViewModel model = new DetailsViewModel();
            Campaign campaigndetails = context.Campaigns.Find(id);
            var campaignComments = context.Comments.Where(c => c.CampaignId.Equals(id)).ToList();
            var campaignDonors = context.Checkouts.Where(c => c.CampaignId.Equals(id)).OrderByDescending(x => x.Checkoutid).ToList();
            var campaignUpdates = context.Update.Where(c => c.CampaignId.Equals(id)).ToList();
            campaigndetails.updates = campaignUpdates;
            campaigndetails.donors = campaignDonors;
            campaigndetails.comments = campaignComments;
            model.CampaignDetails = campaigndetails;


            return model;
        }

    }
}