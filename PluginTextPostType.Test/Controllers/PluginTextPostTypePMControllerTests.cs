using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlogContract;
using MyBlogContract.PostManage;
using Newtonsoft.Json;
using NSubstitute;
using PluginTextPostType.Controllers;
using PluginTextPostType.Models;
using PluginTextPostType.Test;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Microsoft.CSharp.RuntimeBinder;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using PluginTextPostType.Tests;

namespace PluginTextPostType.Controllers.Tests
{
    

    [TestClass()]
    public class PluginTextPostTypePMControllerTests
    {
        [Export] //It needs for MEF metadata loading.
        public IDataStorePostManage _ds;
        PluginTextPostTypePMController controller;

        [TestInitialize]
        public void Init()
        {
            DataStorePostManageFactory.SetDataStore();
            _ds = DataStorePostManageFactory.CurrentDataStore;
            #region filling of _ds
            //IList<IDataStoreRecord> _ds_result
            //    = new List<IDataStoreRecord>();
            //UnicodeEncoding encoding = new UnicodeEncoding();
            //IDataStoreRecord record1 = Substitute.For<IDataStoreRecord>();
            //record1.PostId = 1;
            //record1.PostContentId = 1;
            //record1.tempPostContentId = 0;
            //record1.Status = IDataStoreRecordStatus.None;
            //record1.ContentPluginName = "AnyPlugin";
            //record1.ContentPluginVersion = "1.0";
            //record1.Comment = "AnyComment";
            //record1.ContentData = encoding.GetBytes("MyStringData");
            //record1.IsInGroup = true;
            //record1.Order = 1;
            //_ds_result.Add(record1);

            //IDataStoreRecord record2 = Substitute.For<IDataStoreRecord>();
            //record2.PostId = 1;
            //record2.PostContentId = 2;
            //record2.tempPostContentId = 0;
            //record2.Status = IDataStoreRecordStatus.None;
            //record2.ContentPluginName = "AnyPlugin";
            //record2.ContentPluginVersion = "1.0";
            //record2.Comment = "AnyComment2";
            //record2.ContentData = encoding.GetBytes("MyStringData2");
            //record2.IsInGroup = true;
            //record2.Order = 1;
            //_ds_result.Add(record2);

            //IDataStoreRecord record3 = Substitute.For<IDataStoreRecord>();
            //record3.PostId = 2;
            //record3.PostContentId = 3;
            //record3.tempPostContentId = 0;
            //record3.Status = IDataStoreRecordStatus.None;
            //record3.ContentPluginName = "AnyPlugin";
            //record3.ContentPluginVersion = "1.0";
            //record3.Comment = "AnyComment3";
            //record3.ContentData = encoding.GetBytes("MyStringData3");
            //record3.IsInGroup = false;
            //record3.Order = 1;
            //_ds_result.Add(record3);

            //_ds.GetAllContents().Returns(_ds_result);
            //_ds.GetContent(Arg.Any<int>()).
            //    Returns(x => _ds_result
            //            .Where(r => r.PostContentId == (int)x[0])
            //            .SingleOrDefault());
            ////_ds.WhenForAnyArgs(x => x.Create(Arg.Any<IDataStoreRecord>()))
            ////    .Do(x => _ds_result.Add((IDataStoreRecord)x[0]));

            //_ds.GetNew().Returns(x => Substitute.For<IDataStoreRecord>());

            //_ds.When(x => x.Delete(Arg.Any<int>(), Arg.Any<int>()))
            //    .Do(x => _ds_result
            //                .Remove(_ds_result
            //                    .Where(r=>r.PostContentId == (int)x[0])
            //                    .SingleOrDefault()));

            #endregion
            controller = new PluginTextPostTypePMController(_ds);
            AutoMapperConfig.RegisterMappings();
        }

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void tDisplay_ds_is_null()
        {
            _ds = null;
            PluginTextPostTypePMController controller2 =
                new PluginTextPostTypePMController(_ds);
        }
        #region Display tests
        [TestMethod()]
        public void tDisplay_all_is_correct()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
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
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostId = 1;
            Model.PostContentId = 100;
            Model.AreaName = "PluginTextPostType";

            var result = controller.Display(Model) as ViewResult;
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void tDisplay_PostContentId_not_belong_postid()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostId = 1;
            Model.PostContentId = 3;
            Model.AreaName = "PluginTextPostType";
            var result = controller.Display(Model) as ViewResult;

        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void tDisplay_area_not_this_plugin()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostId = 1;
            Model.PostContentId = 1;
            Model.AreaName = "AnotherPlugin";
            var result = controller.Display(Model) as ViewResult;
        }

        #endregion

        #region Create Tests
        [TestMethod()]
        public void CreateTest()
        {

        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void tCreate_wrong_PostContentId()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostContentId = 1;
            Model.AreaName = "PluginTextPostType";
            Model.CallbackActionName = "something";
            Model.CallbackControllerName = "something";
            Model.List_content_insert_before_Id = "something";
            Model.Update_area_replace_Id = "something";
            Model.OnSuccessRemoveCallback = "something";
            var result = controller.Create(Model) as ViewResult;
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void tCreate_area_not_this_plugin()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostContentId = 0;
            Model.AreaName = "AnotherPlugin";
            Model.CallbackActionName = "something";
            Model.CallbackControllerName = "something";
            Model.List_content_insert_before_Id = "something";
            Model.Update_area_replace_Id = "something";
            Model.OnSuccessRemoveCallback = "something";
            var result = controller.Create(Model) as ViewResult;
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void tCreate_wrong_route_values()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostContentId = 0;
            Model.AreaName = "PluginTextPostType";
            Model.CallbackActionName = "";
            Model.CallbackControllerName = "something";
            Model.List_content_insert_before_Id = "something";
            Model.Update_area_replace_Id = "something";
            Model.OnSuccessRemoveCallback = "something";
            var result = controller.Create(Model) as ViewResult;
        }

        #endregion

        #region Modify tests
        [TestMethod()]
        public void tModify_input_equals_output()
        {
            VmDisplay input = new VmDisplay();
            input.PostContentId = 1;
            input.AreaName = "PluginTextPostType";
            input.CallbackActionName = "something";
            input.CallbackControllerName = "something";
            input.List_content_insert_before_Id = "something";
            input.Update_area_replace_Id = "something";
            input.OnSuccessRemoveCallback = "something";
            var result = controller.Modify(input) as ViewResult;
            var output = result.Model as VmDisplay;
            Assert.AreEqual(input.PostContentId, output.PostContentId);
            Assert.AreEqual(input.AreaName, output.AreaName);
            Assert.AreEqual(input.CallbackActionName, output.CallbackActionName);
            Assert.AreEqual(input.CallbackControllerName, output.CallbackControllerName);
            Assert.AreEqual(input.List_content_insert_before_Id, output.List_content_insert_before_Id);
            Assert.AreEqual(input.Update_area_replace_Id, output.Update_area_replace_Id);
            Assert.AreEqual(input.OnSuccessRemoveCallback, output.OnSuccessRemoveCallback);

        }

        [TestMethod()]
        public void tModify_input_data_equal_output_this_one()
        {
            VmDisplay input = new VmDisplay();
            UnicodeEncoding encoding = new UnicodeEncoding();

            input.PostContentId = 1;
            input.AreaName = "PluginTextPostType";
            input.CallbackActionName = "something";
            input.CallbackControllerName = "something";
            input.List_content_insert_before_Id = "something";
            input.Update_area_replace_Id = "something";
            input.OnSuccessRemoveCallback = "something";
            var result = controller.Modify(input) as ViewResult;
            var output = result.Model as VmDisplay;
            Assert.AreEqual("MyStringData", output.Data);
        }

        [TestMethod()]
        public void tModifyPost_input_data_equal_output_this_one()
        {
            VmDisplay input = new VmDisplay();
            input.PostContentId = 1;
            input.AreaName = "PluginTextPostType";
            input.CallbackActionName = "something";
            input.CallbackControllerName = "something";
            input.List_content_insert_before_Id = "something";
            input.Update_area_replace_Id = "something";
            input.OnSuccessRemoveCallback = "something";
            var result = controller.Modify(input) as ViewResult;
            var output = result.Model as VmDisplay;
            Assert.AreEqual(input.PostContentId, output.PostContentId);
            Assert.AreEqual(input.AreaName, output.AreaName);
            Assert.AreEqual(input.CallbackActionName, output.CallbackActionName);
            Assert.AreEqual(input.CallbackControllerName, output.CallbackControllerName);
            Assert.AreEqual(input.List_content_insert_before_Id, output.List_content_insert_before_Id);
            Assert.AreEqual(input.Update_area_replace_Id, output.Update_area_replace_Id);
            Assert.AreEqual(input.OnSuccessRemoveCallback, output.OnSuccessRemoveCallback);

        }

        [TestMethod()]
        public void tModifyPost_session_exist()
        {
            VmDisplay input = new VmDisplay();
            input.PostContentId = 1;
            input.AreaName = "PluginTextPostType";
            input.CallbackActionName = "something";
            input.CallbackControllerName = "something";
            input.List_content_insert_before_Id = "something";
            input.Update_area_replace_Id = "something";
            input.OnSuccessRemoveCallback = "something";
            input.Data = "Data";

            
            var result = controller.ModifyPost(input) as ViewResult;
            var session_records = _ds.GetAllContents().Count();
            Assert.IsTrue(session_records>0);


        }

        [TestMethod()]
        public void tModifyPost_is_Called()
        {
            VmDisplay input = new VmDisplay();
            input.PostId = 1;
            input.PostContentId = 0;
            input.AreaName = "PluginTextPostType";
            input.CallbackActionName = "something";
            input.CallbackControllerName = "something";
            input.List_content_insert_before_Id = "something";
            input.Update_area_replace_Id = "something";
            input.OnSuccessRemoveCallback = "something";
            input.Data = "Data";

 
            var result = controller.ModifyPost(input) as ViewResult;

            _ds.Received().Modify(Arg.Any<IDataStoreRecord>());
            _ds.Received().Modify(Arg.Is<IDataStoreRecord>(x => x.PostContentId == 0));
            _ds.Received().Modify(Arg.Is<IDataStoreRecord>(x => x.ContentData != null));
            _ds.Received().Modify(Arg.Is<IDataStoreRecord>(x => x.ContentPluginName != null));
            _ds.Received().Modify(Arg.Is<IDataStoreRecord>(x => x.ContentPluginVersion != null));

            Assert.AreEqual(true, input.data_edit_diff_flag);

            Assert.AreEqual("Display", result.ViewName);


          }

        [TestMethod()]
        public void tDeleteContent_by_PostContentId()
        {
            ControllerContextFactory.SetContext();
            controller.ControllerContext = ControllerContextFactory.CurrentContext;
            controller.DeleteContent(2, 0);

            _ds.Received().Delete(Arg.Any<int>(), Arg.Any<int>());
            _ds.Received().Delete(Arg.Is<int>(x=>x==2), Arg.Is<int>(y=>y==0));
        }

      
        #endregion

        [TestMethod()]
        public void tMetadata ()
        {
            AggregateCatalog AggregateCatalog = new AggregateCatalog();
            IEnumerable<AssemblyCatalog> ac = 
                AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(a=>a.FullName.Contains("PluginTextPostType"))
                .Select(a=> new AssemblyCatalog(a));
            foreach (var i in ac)
                AggregateCatalog.Catalogs.Add(i);

            
            CompositionContainer _container = new CompositionContainer(AggregateCatalog);

            var e1 = _container.GetExports<IController, IMetadata>("PluginTextPostType");
            var e2 = e1.Where(e => e.Metadata.ControllerName.Equals("PluginTextPostTypePM"));

            if (e2 != null)
            {
                string Name = e2.Select(m => m.Metadata.Name).SingleOrDefault();
                string Version = e2.Select(m => m.Metadata.Version).SingleOrDefault();
                string ControllerName = e2.Select(m => m.Metadata.ControllerName).SingleOrDefault();
                Type ControllerType = e2.Select(m => m.Metadata.ControllerType).SingleOrDefault();
                string ActionDisplayName = e2.Select(m => m.Metadata.ActionDisplayName).SingleOrDefault();
                string ActionModifyName = e2.Select(m => m.Metadata.ActionModifyName).SingleOrDefault();
                string ActionCreateName = e2.Select(m => m.Metadata.ActionCreateName).SingleOrDefault();
                int prop_count = typeof(IMetadata).GetProperties().Count();

                Assert.AreEqual("PluginTextPostType", Name);
                Assert.AreEqual("1.0",Version);
                Assert.AreEqual("PluginTextPostTypePM", ControllerName);
                Assert.AreEqual(typeof(IPostManager), ControllerType);
                Assert.AreEqual("Display", ActionDisplayName);
                Assert.AreEqual("Modify", ActionModifyName);
                Assert.AreEqual("Create", ActionCreateName);
                Assert.AreEqual(7, prop_count);
            }

            Assert.IsNotNull(e2);
            Assert.AreEqual(1,e2.Count());
            
        }
    }
}