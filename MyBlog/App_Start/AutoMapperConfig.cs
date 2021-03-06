﻿using AutoMapper;
using MyBlog.Models;
using MyBlog.ViewModels;
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.FullContent;
using MyBlogContract.PostManage;
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

            Mapper.CreateMap<PostContent, IDataStoreRecord>();

            Mapper.CreateMap<IDataStoreRecord, PostContent>();

            Mapper.CreateMap<IDeItemBand, IDeFullContent>();


            Mapper.CreateMap<Post, PostVm>()
                .ForMember(dst => dst.PostContents, x => x.Ignore());
            Mapper.CreateMap<PostVm, Post>()
                .ForMember(dst => dst.PostContents, x => x.Ignore());

            Mapper.CreateMap<PostVm, PostGroupVm>()
                .ReverseMap();


            Mapper.CreateMap<IDsTag, Tag>()
                .ReverseMap();

        }
    }
}