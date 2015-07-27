using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.DomenModels
{
    public class Registration
    {
        public int Id { get; set; }
        [MaxLength(100)]
        [MinLength(4)]
        [Required]
        public string FullName { get; set; }
        [EmailAddress]
        [Required]
        public string Mail { get; set; }
        public bool IsMailSended { get; set; }
        [Range(typeof(DateTime),"2015-07-25", "2015-08-25",ErrorMessage = "Date of mail sendig shuld be in range...")]
        DateTime DateOfMailSended { get; set; }
        public bool IsDeliveryError { get; set; }
        public bool IsUserBack { get; set; }
        public bool IsUserChangePassword { get; set; }
        public bool IsUserConfirmRegistration { get; set; }

        public User User { get; set; }
        [Timestamp]
        public Byte[] RowVer { get; set; }
    }
}