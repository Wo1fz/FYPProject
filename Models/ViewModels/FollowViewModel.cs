using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FYPProject.Models
{
    public class FollowViewModel
    {          
        [Required]          
        public int CampaignId { get; set; }
        public string Id { get; set; }
    }

    
}