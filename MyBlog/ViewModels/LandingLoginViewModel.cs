﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.ViewModels
{
    public class LandingLoginViewModel
    {
        [Display(Name ="E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmailLog { get; set; }
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [Display(Name = "Запомнить меня")]
        [Required]
        public bool RememberMe { get; set; }
    }
}