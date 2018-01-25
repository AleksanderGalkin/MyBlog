using MyBlogContract;
using PluginTag.DataExchangeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PluginTag.ViewModels
{
    public class VmDtoTagItem: IDsTag
    {
        public int TagId { get; set; }
        public string TagName { get; set; }

    }
    public class VmDtoTag : DtoTag, IDto
    {

        public List<VmDtoTagItem> all_tags { get; set; }
        public List<VmDtoTagItem> post_tags { get; set; }

        public VmDtoTag()
        {
            all_tags = new List<VmDtoTagItem>();
            post_tags = new List<VmDtoTagItem>();
        }

    }
}