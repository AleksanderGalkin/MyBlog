using MyBlogContract;
using MyBlogContract.TagCloud;
using PluginTagCloud.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PluginTagCloud.ViewModels
{
    public class VmDtoTagCloudItem: IDsTagCloud
    {


        public int TagId { get; set; }
        public string TagName { get; set; }
        public int Frequency { get; set; }

    }
    public class VmDtoTagCloud : DtoTagCloud
    {

        public List<VmDtoTagCloudItem> cloud_tags { get; set; }

        public VmDtoTagCloud()
        {
            cloud_tags = new List<VmDtoTagCloudItem>();
        }

    }
}