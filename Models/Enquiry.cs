using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoogleLogin.Models
{
    public partial class Enquiry
    {
        public int EnquiryId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
