using AutoMapper;

using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.ContentGroup;
using MyBlogContract.FullContent;
using MyBlogContract.PostManage;
using MyBlogContract.TagManage;
using PluginTag.SelectListData;
using PluginTag.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PluginTag
{ 
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<IDtoTag, VmDtoTag>()
                .ReverseMap();

            Mapper.CreateMap<IDsTag, VmDtoTagItem>()
                .ReverseMap();

            Mapper.CreateMap<SelectListStoreModel, VmDtoDisplayIn>()
                .ReverseMap();

            Mapper.CreateMap<DtoDisplayIn, VmDtoDisplayIn>()
                .ReverseMap();




        }
    }
}