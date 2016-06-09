using AutoMapper;

using MyBlogContract;
using MyBlogContract.Band;
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

            Mapper.CreateMap<IDEModelDisplay, VmDisplay>()
                .ReverseMap();

            Mapper.CreateMap<IDEModelPostManage, VmDisplay>()
                .ReverseMap();

            Mapper.CreateMap<IDEModelFullContent, VmDisplay>()
                .ReverseMap();

            Mapper.CreateMap<IDataStoreRecord, VmDisplay>()
                .ReverseMap();
                

        }
    }
}