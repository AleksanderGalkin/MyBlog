using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.DomainModels
{
    public class Registration
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [MaxLength(100)]
        [MinLength(4)]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name ="Полное имя",Prompt ="placeholder")]
        public string FullName { get; set; }

        [EmailAddress]
        [Required]
        [Display(Name = "E-mail")]
        [UIHint("EmailAddress")]
        public string Mail { get; set; }

        [Display(Name = "Отправлено")]
        [UIHint("Boolean")]
        public bool IsMailSended { get; set; }

        //[Range(typeof(DateTime),"2015-07-25", "2015-08-25",ErrorMessage = "Date of mail sendig shuld be in range...")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode =true)] //@"{0:dd\/MM\/yyyy}"
        [Display(Name = "Дата отправки")]
        public DateTime DateOfMailSended { get; set; }

        [Display(Name = "Ошибка")]
        [UIHint("Boolean")]
        public bool IsDeliveryError { get; set; }

        [Display(Name = "Пользователь вернулся")]
        [UIHint("Boolean")]
        public bool IsUserBack { get; set; }

        [Display(Name = "Пароль изменён")]
        [UIHint("Boolean")]
        public bool IsUserChangePassword { get; set; }

        [Display(Name = "Регистрация подтверждена")]
        [UIHint("Boolean")]
        public bool IsUserConfirmRegistration { get; set; }

        public User User { get; set; }
        [Timestamp]
        public Byte[] RowVer { get; set; }
    }
}