using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace FYPProject.Models
{
        public class ChangeInfoViewModel
        {

            [Required]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }

            [Required]
            [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
            [Display(Name = "Email")]
            public string Email { get; set; }

        }
}