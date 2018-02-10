using AutoMapper;

using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.ContentGroup;
using MyBlogContract.FullContent;
using MyBlogContract.PostManage;
using MyBlogContract.TagCloud;
using MyBlogContract.TagManage;
using PluginTagCloud.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PluginTagCloud
{ 
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {

            Mapper.CreateMap<IDtoTagCloud, VmDtoTagCloud>()
                .ReverseMap();
            Mapper.CreateMap<IDsTagCloud, VmDtoTagCloudItem>()
                .ReverseMap();

        }
    }
}