using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FYPProject.Models
{
    public class EmailViewModel
    {
        [DataType(DataType.EmailAddress), Display(Name = "To")]
        [Required]
        public string ToEmail { get; set; }

        [DataType(DataType.EmailAddress), Display(Name = "From")]
        [Required]
        public string FromEmail { get; set; }

        [Display(Name = "Body")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string EMailBody { get; set; }

        [Display(Name = "Subject")]
        [Required]
        public string EmailSubject { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "CC")]
        public string EmailCC { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "BCC")]
        public string EmailBCC { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Your Email")]
        [Required]
        public string EmailAddress { get; set; }
    }
}