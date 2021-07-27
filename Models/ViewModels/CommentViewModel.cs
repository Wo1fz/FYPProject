using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FYPProject.Models
{
    public class CommentViewModel
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public int CampaignId { get; set; }

        public string OwnerId { get; set; }
    }
}