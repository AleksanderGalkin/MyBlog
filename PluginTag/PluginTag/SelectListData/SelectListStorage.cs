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


    [Export("PluginTag", typeof(IDataStore<SelectListStoreModel>))]
    public class SelectListStorage : IDataStore<SelectListStoreModel>
    {
        public SelectListStorage() : base()
        {
        }

    }

    public class SelectListStoreModel : IStoreModel
    {

        public IList<SelectListStoreModelItem> all_items { get; set; }
        public IList<SelectListStoreModelItem> select_items { get; set; }

        public SelectListStoreModel()
        {
            all_items = new List<SelectListStoreModelItem>();
            select_items = new List<SelectListStoreModelItem>();
        }
    }

    public class SelectListStoreModelItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Select { get; set; }

    }


    public class SelectListStoreModelItemComparer : IEqualityComparer<SelectListStoreModelItem>
    {

        public bool Equals(SelectListStoreModelItem x, SelectListStoreModelItem y)
        {
            //Check whether the objects are the same object. 
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether the products' properties are equal. 
            return x != null && y != null && x.Id.Equals(y.Id) && x.Name.Equals(y.Name);
        }

        public int GetHashCode(SelectListStoreModelItem obj)
        {
            //Get hash code for the Name field if it is not null. 
            int hashProductName = obj.Name == null ? 0 : obj.Name.GetHashCode();

            //Get hash code for the Code field. 
            int hashProductCode = obj.Id.GetHashCode();

            //Calculate the hash code for the product. 
            return hashProductName ^ hashProductCode;
        }
    }


}