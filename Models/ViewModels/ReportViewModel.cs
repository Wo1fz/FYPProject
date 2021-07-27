using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FYPProject.Models
{
    public class ReportViewModel
    {
        [Required]
        public int CampaignId { get; set; }
        [Required]
        public string ReporterId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Please Elaborate")]
        public string Body { get; set; }
        [Required]
        public string Reason { get; set; }
    }
}