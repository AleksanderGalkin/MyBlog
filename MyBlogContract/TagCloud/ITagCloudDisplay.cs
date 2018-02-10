using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyBlogContract.TagCloud
{

    public abstract class ITagCloudDisplay : Controller
    {
        protected IDataStore<IDsTagCloudModel> _ds;

        protected ITagCloudDisplay(IDataStore<IDsTagCloudModel> DataStore)
        {
            if (DataStore == null)
                throw new NullReferenceException("DataStore reference must be not null");
            _ds = DataStore;
        }

        public abstract ActionResult Display(IDtoTagCloud Model);
    }

}
