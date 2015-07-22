using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyBlog.DomenModels
{
    public class User
    {
        [Key]
        [ForeignKey("Registration")]
        public int Id { get; set; }
        public string FullName { get; set;}
        public string Mail { get; set;}
        public int RegistrationId { get; set; }
        public Registration Registration { get; set;}
        bool IsNotificarionAllowed { get; set;}
        string Sex { get; set;}
        
    }
}