using AutoMapper;

using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.ContentGroup;
using MyBlogContract.FullContent;
using MyBlogContract.PostManage;
using PluginTextPostType.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PluginTextPostType
{ 
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {

            Mapper.CreateMap<IDeGroupBand, VmManage>()
                .ReverseMap();

            Mapper.CreateMap<IDEModelPostManage, VmManage>()
                .ReverseMap();

            Mapper.CreateMap<IDeFullContent, VmManage>()
                .ReverseMap();

            Mapper.CreateMap<IDataStoreRecord, VmManage>()
                .ReverseMap();

            Mapper.CreateMap<IDeGroupBand, VmItem>()
                           .ReverseMap();

            Mapper.CreateMap<IDEModelPostManage, VmItem>()
                .ReverseMap();

            Mapper.CreateMap<IDeFullContent, VmItem>()
                .ReverseMap();

            Mapper.CreateMap<IDataStoreRecord, VmItem>()
                .ReverseMap();

            Mapper.CreateMap<IDeContentGroup, VmContentGroup>()
                .ReverseMap();
            Mapper.CreateMap<IDataStoreRecord, VmContentGroup>()
                 .ReverseMap();


        }
    }
}