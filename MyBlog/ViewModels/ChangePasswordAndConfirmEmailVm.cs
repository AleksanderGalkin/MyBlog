using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.ViewModels
{
    public class ChangePasswordAndConfirmEmailVm
    {
        [Required]
        [Display(Name ="Новый пароль")]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }
        [Required]
        [Display(Name = "Повторите ввод пароля")]
        [DataType(DataType.Password)]
        [Compare("newPassword",ErrorMessage ="The password and confirmation password do not match")]
        public string confirmNewPassword { get; set; }
        [Required]
        public string userId { get; set; }
        [Required]
        public string code { get; set; }
    }
}