using AutoMapper;
using MyBlogContract;
using MyBlogContract.Band;
using MyBlogContract.FullContent;
using MyBlogContract.PostManage;
using PluginImagePostType.Models;


namespace PluginImagePostType
{ 
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {

            Mapper.CreateMap<IDeGroupModelDisplay, VmDisplay>()
                .ReverseMap();

            Mapper.CreateMap<IDEModelPostManage, VmDisplay>()
                .ReverseMap();

            Mapper.CreateMap<IDEModelFullContent, VmDisplay>()
                .ReverseMap();

            Mapper.CreateMap<IDataStoreRecord, VmDisplay>()
                .ReverseMap();

            Mapper.CreateMap<IDEModelPostManage, GroupVmDisplay>()
                .ReverseMap();

        }
    }
}