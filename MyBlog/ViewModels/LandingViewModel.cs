using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.ViewModels
{
    public class LandingViewModel
    {
        [Display(Name ="Имя")]
        [Required]
        public string FullName { get; set; }
        [Display(Name ="E-Mail")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        [Display(Name ="Уведомлять меня о новинках")]
        [Required]
        public bool NotifyMe { get; set; }
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [Display(Name = "Запомнить меня")]
        [Required]
        public bool RememberMe { get; set; }
    }
}