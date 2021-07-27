using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FYPProject.Models
{
    public class Checkout
    {
        public int Checkoutid { get; set; }
        public string CheckoutToken { get; set; }
        public int Amount { get; set; }
        public int CampaignId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string OrganiserId { get; set; }
        public string Anonymous { get; set; }
    }
}