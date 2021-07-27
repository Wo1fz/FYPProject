using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace FYPProject.Models
{
    public class CreateCampaignViewModel
    {
        [Key]
        public int CampaignId { get; set; }
        [Required]
        public double? DonationGoal { get; set; }
        [Required]
        public double CurrentDonation { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        [Required]
        public int CampaignDuration { get; set; }
        [Required]
        public string OwnerId { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string OwnerUsername { get; set; }
        [Required]
        public string Body { get; set; }
        [Required,StringLength(6, MinimumLength = 6, ErrorMessage = "Postal Code must be 6 digits.")]
        public string PostalCode { get; set; }
        [Required]
        public string Address { get; set; }
        public string Beneficiary { get; set; }
        public string CoverImage { get; set; }
        public string MultipleImages { get; set; }
        [NotMapped]
        public HttpPostedFileBase CoverImageFile { get; set; }
        [NotMapped]
        public IEnumerable<HttpPostedFileBase> MultipleImagesFile { get; set; }
        [Required]
        public int Donors { get; set; }
        [Required]
        public int Shares { get; set; }
        [Required]
        public int Followers { get; set; }
        [Required]
        public int Reports { get; set; }
    }
    public class DetailsViewModel
    {
        public Campaign CampaignDetails { get; set; }

    }
}