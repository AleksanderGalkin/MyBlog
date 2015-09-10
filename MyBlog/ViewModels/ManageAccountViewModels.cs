using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.ViewModels
{
    public class LoginVm
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
    public class RegisterVm
    {
        [Display(Name = "Полное имя")]
        [Required]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Длина имени должна быть от 4 до 100 символов")]
        public string FullName { get; set; }
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmailReg { get; set; }
        [Display(Name = "Уведомлять меня о новинках")]
        [Required]
        public bool NotifyMe { get; set; }

    }
    public class ForgottenPasswordVm
    {
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
    }

    public class ChangePasswordAndConfirmEmailVm
    {
        [Required]
        [Display(Name = "Новый пароль")]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }
        [Required]
        [Display(Name = "Повторите ввод пароля")]
        [DataType(DataType.Password)]
        [Compare("newPassword", ErrorMessage = "The password and confirmation password do not match")]
        public string confirmNewPassword { get; set; }
        [Required]
        public string userId { get; set; }
        [Required]
        public string code { get; set; }
    }

}