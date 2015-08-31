using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.ViewModels
{
    public class ForgottenPasswordVm
    {
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
    }
}