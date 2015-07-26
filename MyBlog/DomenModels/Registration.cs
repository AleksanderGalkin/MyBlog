using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.DomenModels
{
    public class Registration
    {
        public int RegistrationId { get; set; }
        public string FullName { get; set; }
        public string Mail { get; set; }
        public bool IsMailSended { get; set; }
        DateTime DateOfMailSended { get; set; }
        public bool IsDeliveryError { get; set; }
        public bool IsUserBack { get; set; }
        public bool IsUserChangePassword { get; set; }
        public bool IsUserConfirmRegistration { get; set; }
        public bool IsNotificationAllowed { get; set; }

        public virtual User User { get; set; }
    }
}