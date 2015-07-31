using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyBlog.DomainModels
{
    public class User
    {
        [Key]
        [ForeignKey("Registration")]
        public int Id { get; set; }
        public Registration Registration { get; set;}
        public bool IsNotificarionAllowed { get; set;}
        [MaxLength(6)]
        [MinLength(4)]
     
        public string Sex { get; set;}

        [Timestamp]
        public Byte[] RowVer { get; set; }

    }
}