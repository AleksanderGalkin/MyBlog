using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlogContract;
using MyBlogContract.FullContent;
using NSubstitute;
using PluginTextPostType.Controllers;
using PluginTextPostType.Controllers.Tests;
using PluginTextPostType.DataExchangeModels;
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
    public class PluginFullContentControllerTests
    {
        IDataStoreFullContent _ds;
        PluginFullContentController controller;

        [TestInitialize]
        public void Init()
        {
            _ds = Substitute.For<IDataStoreFullContent>();
            #region filling of _ds
            IList<IDataStoreRecord> _ds_result
                = new List<IDataStoreRecord>();
            UnicodeEncoding encoding = new UnicodeEncoding();
            IDataStoreRecord record1 = Substitute.For<IDataStoreRecord>();
                record1.PostId = 1;
                record1.PostContentId = 1;
                record1.PostContentIdForNewRecords = 0;
                record1.Status = IDataStoreRecordStatus.None;
                record1.ContentPluginName = "AnyPlugin";
                record1.ContentPluginVersion = "1.0";
                record1.Comment = "AnyComment";
                record1.ContentData = encoding.GetBytes("MyStringData");
            _ds_result.Add(record1);

            IDataStoreRecord record2 = Substitute.For<IDataStoreRecord>();
                record2.PostId = 1;
                record2.PostContentId = 2;
                record2.PostContentIdForNewRecords = 0;
                record2.Status = IDataStoreRecordStatus.None;
                record2.ContentPluginName = "AnyPlugin";
                record2.ContentPluginVersion = "1.0";
                record2.Comment = "AnyComment2";
                record2.ContentData = encoding.GetBytes("MyStringData2");
            _ds_result.Add(record2);

            IDataStoreRecord record3 = Substitute.For<IDataStoreRecord>();
                record3.PostId = 2;
                record3.PostContentId = 3;
                record3.PostContentIdForNewRecords = 0;
                record3.Status = IDataStoreRecordStatus.None;
                record3.ContentPluginName = "AnyPlugin";
                record3.ContentPluginVersion = "1.0";
                record3.Comment = "AnyComment3";
                record3.ContentData = encoding.GetBytes("MyStringData3");
            _ds_result.Add(record3);

            _ds.Get().Returns(_ds_result);
            int argumentUsed = 2;
            var e1 = _ds_result
                        .Where(r => r.PostContentId == argumentUsed)
                        .SingleOrDefault();
            _ds.Get(Arg.Any<int>()).
                Returns(x=>_ds_result
                        .Where(r => r.PostContentId == (int)x[0] )
                        .SingleOrDefault());
            #endregion
            controller = new PluginFullContentController(_ds);
            AutoMapperConfig.RegisterMappings();
        }

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void tDisplay_ds_is_null()
        {
            _ds = null;
            PluginFullContentController controller_thrown =
                new PluginFullContentController(_ds);
        }

        [TestMethod()]
        public void tDisplay_all_is_correct()
        {
            IDEModelFullContent Model = Substitute.For<IDEModelFullContent>();
            Model.PostId = 1;
            Model.PostContentId = 2;
            Model.AreaName = "PluginTextPostType";
            var result = controller.Display(Model) as ViewResult;
            var output_model = result.Model as VmDisplay;
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(1, output_model.PostId);
            Assert.AreEqual(2, output_model.PostContentId);
            Assert.AreEqual("PluginTextPostType", output_model.AreaName);
            Assert.AreEqual("AnyComment2", output_model.Comment);
            Assert.AreEqual("MyStringData2", output_model.Data);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void tDisplay_wrong_PostContentId()
        {
            IDEModelFullContent Model = Substitute.For<IDEModelFullContent>();
            Model.PostId = 1;
            Model.PostContentId = 100;
            Model.AreaName = "PluginTextPostType";

            var result = controller.Display(Model) as ViewResult;
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void tDisplay_PostContentId_not_belong_postid()
        {
            IDEModelFullContent Model = Substitute.For<IDEModelFullContent>();
            Model.PostId = 1;
            Model.PostContentId = 3;
            Model.AreaName = "PluginTextPostType";
            var result = controller.Display(Model) as ViewResult;

        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void tDisplay_area_not_this_plugin()
        {
            IDEModelFullContent Model = Substitute.For<IDEModelFullContent>();
            Model.PostId = 1;
            Model.PostContentId = 1;
            Model.AreaName = "AnotherPlugin";
            var result = controller.Display(Model) as ViewResult;
        }
    }
}