using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.ViewModels
{
    public class RegisterVm
    {
        [Display(Name ="Полное имя")]
        [Required]
        [StringLength(100,MinimumLength =4, ErrorMessage = "Длина имени должна быть от 4 до 100 символов")]
        public string FullName { get; set; }
        [Display(Name ="E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmailReg { get; set; }
        [Display(Name ="Уведомлять меня о новинках")]
        [Required]
        public bool NotifyMe { get; set; }

    }
}