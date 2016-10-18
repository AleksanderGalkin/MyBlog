using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlog.Controllers;
using MyBlog.Infrustructure;
using MyBlog.Tests;
using MyBlogContract;
using MyBlogContract.Band;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyBlog.Controllers.Tests
{
    [TestClass()]
    public class BandControllerTests
    {
        IDataStoreBand _ds;
        BandController controller;

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
                Returns(x => _ds_result
                        .Where(r => r.PostContentId == (int)x[0])
                        .SingleOrDefault());
            #endregion

            IUnitOfWork UoW = Substitute.For<IUnitOfWork>();
            var a = DbContextFactory.DbContext;
            ((IUnitOfWork)UoW).db.Returns(a);
            controller = new BandController(UoW,_ds);
        }

        [TestMethod()]
        public void ShowContentTest()
        {
            var result = controller.Index() as ViewResult;
            Assert.Fail();
        }
    }
}