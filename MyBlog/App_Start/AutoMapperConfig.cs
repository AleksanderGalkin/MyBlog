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
            Mapper.CreateMap<PostContent, PostContent>()
                .ForMember(dst => dst.Post, src => src.Ignore());

            Mapper.CreateMap<PostContent, ContentTextVm>()
                .ForMember(dst => dst.ContentData,src=>src.Ignore()); 
            Mapper.CreateMap<PostContent, ContentImageVm>()
                 .ForMember(dst => dst.ContentData, src => src.Ignore());

            Mapper.CreateMap<ContentTextVm,PostContent > ()
                .ForMember(dst => dst.ContentData, src => src.Ignore());
            Mapper.CreateMap<ContentImageVm, PostContent>()
                .ForMember(dst => dst.ContentData, src => src.Ignore());

            Mapper.CreateMap<Post, PostDispVm>()
                .ForMember(dst => dst.PostContents, x => x.Ignore());
            Mapper.CreateMap<PostDispVm, Post>()
                .ForMember(dst => dst.PostContents, x => x.Ignore());


            Mapper.CreateMap<Post, PostEditVm>()
                .ForMember(dst => dst.PostContents, x => x.Ignore());
            Mapper.CreateMap<PostEditVm, Post>()
                .ForMember(dst => dst.PostContents, x => x.Ignore());
        }
    }
}