using MyBlogContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PluginTag.SelectListData
{
    public class VmDtoDisplayIn : DtoDisplayIn
    {

        public IList<SelectListStoreModelItem> all_items { get; set; }
        public IList<SelectListStoreModelItem> select_items { get; set; }

        public VmDtoDisplayIn()
        {
            all_items = new List<SelectListStoreModelItem>();
            select_items = new List<SelectListStoreModelItem>();
        }

    }
}