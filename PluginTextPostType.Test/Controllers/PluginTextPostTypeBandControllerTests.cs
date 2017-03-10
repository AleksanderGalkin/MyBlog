using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlogContract;
using MyBlogContract.Band;
using NSubstitute;
using PluginTextPostType.Controllers;
using PluginTextPostType.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PluginTextPostType.Controllers.Tests
{
    [TestClass()]
    public class PluginTextPostTypeBandControllerTests
    {

        IDataStoreBand _ds;
        PluginTextPostTypeBandController controller;

        [TestInitialize]
        public void Init()
        {
            _ds = Substitute.For<IDataStoreBand>();
            #region filling of _ds
            IList<IDataStoreRecord> _ds_result
                = new List<IDataStoreRecord>();
            UnicodeEncoding encoding = new UnicodeEncoding();
            IDataStoreRecord record1 = Substitute.For<IDataStoreRecord>();
            record1.PostId = 1;
            record1.PostContentId = 1;
            record1.tempPostContentId = 0;
            record1.Status = IDataStoreRecordStatus.None;
            record1.ContentPluginName = "AnyPlugin";
            record1.ContentPluginVersion = "1.0";
            record1.Comment = "AnyComment";
            record1.ContentData = encoding.GetBytes("MyStringData");
            record1.Order = 1;
            _ds_result.Add(record1);

            IDataStoreRecord record2 = Substitute.For<IDataStoreRecord>();
            record2.PostId = 1;
            record2.PostContentId = 2;
            record2.tempPostContentId = 0;
            record2.Status = IDataStoreRecordStatus.None;
            record2.ContentPluginName = "AnyPlugin";
            record2.ContentPluginVersion = "1.0";
            record2.Comment = "AnyComment2";
            record2.ContentData = encoding.GetBytes("MyStringData2");
            record2.Order = 1;
            _ds_result.Add(record2);

            IDataStoreRecord record3 = Substitute.For<IDataStoreRecord>();
            record3.PostId = 2;
            record3.PostContentId = 3;
            record3.tempPostContentId = 0;
            record3.Status = IDataStoreRecordStatus.None;
            record3.ContentPluginName = "AnyPlugin";
            record3.ContentPluginVersion = "1.0";
            record3.Comment = "AnyComment3";
            record3.ContentData = encoding.GetBytes("MyStringData3");
            record3.Order = 1;
            _ds_result.Add(record3);

            _ds.GetAllContents().Returns(_ds_result);
            _ds.GetContent(Arg.Any<int>()).
                Returns(x => _ds_result
                        .Where(r => r.PostContentId == (int)x[0])
                        .SingleOrDefault());
            _ds.GetModPost(Arg.Any<int>()).
            Returns(x => _ds_result
            .Where(r => r.PostId == (int)x[0]));
            _ds.GetGroupContent(Arg.Any<int>(), Arg.Any<int>()).
            Returns(x => _ds_result
            .Where(r => (r.PostId == (int)x[0] && r.Order == (int)x[1])));
            #endregion
            controller = new PluginTextPostTypeBandController(_ds);
            AutoMapperConfig.RegisterMappings();
        }

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void tDisplay_ds_is_null()
        {
            _ds = null;
            PluginTextPostTypeBandController controller2 =
                new PluginTextPostTypeBandController(_ds);
        }

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void tDisplay_Model_is_null()
        {
            controller.Display(null);
        }


        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void tDisplay_area_not_this_plugin()
        {
            IDeGroupModelDisplay Model = Substitute.For<IDeGroupModelDisplay>();
            Model.PostId = 1;
            Model.AreaName = "AnotherPlugin";
            var result = controller.Display(Model) as ViewResult;
        }


        [TestMethod()]
        public void tDisplay_all_is_correct()
        {
            IDeGroupModelDisplay Model = Substitute.For<IDeGroupModelDisplay>();
            Model.PostId = 1;
            Model.Order = 1;
            Model.AreaName = "PluginTextPostType";
            var result = controller.Display(Model) as ViewResult;
            var output_model = result.Model as IEnumerable<GroupVmDisplay>;
            Assert.IsNotNull(output_model);
            Assert.IsInstanceOfType(output_model, typeof(IEnumerable<GroupVmDisplay>));
            Assert.IsTrue(output_model.Count()>0, "output_model.Count()>0");
            var min_count_group 
                = output_model.Min(x => x.VmDisplays.Count());
            Assert.IsTrue(min_count_group > 0
                , "min_count_group > 0");
            var max_count_postid_in_group 
                = output_model.Max(x => x.VmDisplays.GroupBy(p=>p.PostId).Count());
            Assert.IsFalse(max_count_postid_in_group < 1 || max_count_postid_in_group > 1
                , "max_count_postid_in_group < 1 || max_count_postid_in_group > 1");
            var min_count_postid_in_group
                = output_model.Min(x => x.VmDisplays.GroupBy(p => p.PostId).Count());
            Assert.IsFalse(min_count_postid_in_group < 1 || min_count_postid_in_group > 1
                , "min_count_postid_in_group < 1 || min_count_postid_in_group > 1");
            var count_empty_comments
                = output_model.SelectMany(x => x.VmDisplays.Select(c => c.Comment))
                .Where(e => string.IsNullOrWhiteSpace(e))
                .Count();
            Assert.IsTrue(count_empty_comments == 0
                , "count_empty_comments == 0");
            var count_empty_data
                = output_model.SelectMany(x => x.VmDisplays.Select(c => c.Data))
                .Where(e => string.IsNullOrWhiteSpace(e))
                .Count();
            Assert.IsTrue(count_empty_data == 0
                , "count_empty_data == 0");
        }
    }
}