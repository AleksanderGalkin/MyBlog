using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyBlog.DomenModels
{
    public class User
    {   [Key]
        [ForeignKey("Registration")]
        public int RegistrationId { get; set; }
        public virtual Registration Registration { get; set; }

        public string FullName { get; set;}
        public string Mail { get; set;}
        bool IsNotificarionAllowed { get; set;}
        [MaxLength(6)]
        [MinLength(4)]
        string Sex { get; set;}
        
    }
}