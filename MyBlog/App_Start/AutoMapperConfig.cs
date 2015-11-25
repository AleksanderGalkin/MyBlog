using AutoMapper;
using MyBlog.Models;
using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<PostContent, ContentTextDispVm>(); 
            Mapper.CreateMap<PostContent, ContentImageDispVm>();
            Mapper.CreateMap<PostContent, ContentTextEditVm>();
            Mapper.CreateMap<PostContent, ContentImageEditVm>();
           // Mapper.CreateMap<PostContent, IContentTypeEditVm>();
        }
    }
}