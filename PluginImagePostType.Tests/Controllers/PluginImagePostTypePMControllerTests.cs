using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlogContract;
using MyBlogContract.PostManage;
using Newtonsoft.Json;
using NSubstitute;
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
using PluginImagePostType.Controllers;
using PluginImagePostType;
using PluginImagePostType.Models;
using PluginImagePostType.Test;
using PluginImagePostType.Tests;
using System.Drawing.Imaging;
using System.Drawing;

namespace PluginImagePostType.Controllers.Tests
{
    

    [TestClass()]
    public class PluginImagePostTypePMControllerTests
    {
        [Export] //It needs for MEF metadata loading.
        public IDataStorePostManage _ds;
        PluginImagePostTypePMController controller;
        HttpPostedFileBase[] files;



        [TestInitialize]
        public void Init()
        {
            _ds = Substitute.For<IDataStorePostManage>();
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
            MemoryStream ms = new MemoryStream();
            ImageFactory.GetImage(record1.PostContentId).Save(ms,ImageFormat.Bmp);
            record1.ContentData = ms.ToArray();
            record1.IsInGroup = true;
            record1.Order = 1;
            _ds_result.Add(record1);

            IDataStoreRecord record2 = Substitute.For<IDataStoreRecord>();
            record2.PostId = 1;
            record2.PostContentId = 2;
            record2.PostContentIdForNewRecords = 0;
            record2.Status = IDataStoreRecordStatus.None;
            record2.ContentPluginName = "AnyPlugin";
            record2.ContentPluginVersion = "1.0";
            record2.Comment = "AnyComment2";
            ms = new MemoryStream();
            ImageFactory.GetImage(record2.PostContentId).Save(ms, ImageFormat.Bmp);
            record2.ContentData = ms.ToArray();
            record2.IsInGroup = true;
            record2.Order = 1;
            _ds_result.Add(record2);

            IDataStoreRecord record3 = Substitute.For<IDataStoreRecord>();
            record3.PostId = 2;
            record3.PostContentId = 3;
            record3.PostContentIdForNewRecords = 0;
            record3.Status = IDataStoreRecordStatus.None;
            record3.ContentPluginName = "AnyPlugin";
            record3.ContentPluginVersion = "1.0";
            record3.Comment = "AnyComment3";
            ms = new MemoryStream();
            ImageFactory.GetImage(record3.PostContentId).Save(ms, ImageFormat.Bmp);
            record3.ContentData = ms.ToArray();
            record3.IsInGroup = false;
            record3.Order = 1;
            _ds_result.Add(record3);

            _ds.Get().Returns(_ds_result);
            _ds.Get(Arg.Any<int>()).
                Returns(x => _ds_result
                        .Where(r => r.PostContentId == (int)x[0])
                        .SingleOrDefault());
            _ds.WhenForAnyArgs(x => x.Create(Arg.Any<IDataStoreRecord>()))
                .Do(x => _ds_result.Add((IDataStoreRecord)x[0]));

            _ds.GetNew().Returns(x => Substitute.For<IDataStoreRecord>());

            _ds.When(x => x.Delete(Arg.Any<int>(), Arg.Any<int>()))
                .Do(x => _ds_result
                            .Remove(_ds_result
                                .Where(r=>r.PostContentId == (int)x[0])
                                .SingleOrDefault()));

            #endregion

            HttpPostedFileBase hpfb = Substitute.For<HttpPostedFileBase>();
            hpfb.FileName.Returns("File1.jpg");
            Image newFile = ImageFactory.GetImage(4);
            MemoryStream ms2 = new MemoryStream();
            newFile.Save(ms2, ImageFormat.Jpeg);
            hpfb.InputStream.Returns(ms);
            hpfb.ContentLength.Returns((int)ms.Length);
            hpfb.ContentType.Returns("image/jpeg");
            files = new HttpPostedFileBase[] { hpfb };

            controller = new PluginImagePostTypePMController(_ds);
            AutoMapperConfig.RegisterMappings();
        }

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void tController_ds_is_null()
        {
            _ds = null;
            PluginImagePostTypePMController controller2 =
                new PluginImagePostTypePMController(_ds);
        }

        #region Display tests

        [TestMethod()]
        public void tDisplay_all_is_correct()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostId = 1;
            Model.PostContentId = 2;
            Model.area = "PluginImagePostType";
            var result = controller.Display(Model) as ViewResult;
            var output_model = result.Model as VmDisplay;
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(1, output_model.PostId);
            Assert.AreEqual(2, output_model.PostContentId);
            Assert.AreEqual("PluginImagePostType", output_model.area);
            Assert.AreEqual("AnyComment2", output_model.Comment);

            string imageBase64 = Convert.ToBase64String(_ds.Get(2).ContentData);
            var output_string_data = string.Format("data:image/jpeg;base64,{0}", imageBase64);
            Assert.AreEqual(output_string_data, output_model.Data);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void tDisplay_wrong_PostContentId()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostId = 1;
            Model.PostContentId = 100;
            Model.area = "PluginImagePostType";

            var result = controller.Display(Model) as ViewResult;
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void tDisplay_PostContentId_not_belong_postid()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostId = 1;
            Model.PostContentId = 3;
            Model.area = "PluginImagePostType";
            var result = controller.Display(Model) as ViewResult;

        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void tDisplay_area_not_this_plugin()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostId = 1;
            Model.PostContentId = 1;
            Model.area = "AnotherPlugin";
            var result = controller.Display(Model) as ViewResult;
        }

        #endregion

        #region Create Tests


        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void tCreate_wrong_PostContentId()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostContentId = 1;
            Model.area = "PluginImagePostType";
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
            Model.area = "AnotherPlugin";
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
            Model.area = "PluginImagePostType";
            Model.CallbackActionName = "";
            Model.CallbackControllerName = "something";
            Model.List_content_insert_before_Id = "something";
            Model.Update_area_replace_Id = "something";
            Model.OnSuccessRemoveCallback = "something";
            var result = controller.Create(Model) as ViewResult;
        }

        #endregion


        #region LoadFiles Tests


        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void tLoadFiles_wrong_PostContentId()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostContentId = 1;
            Model.area = "PluginImagePostType";
            Model.CallbackActionName = "something";
            Model.CallbackControllerName = "something";
            Model.List_content_insert_before_Id = "something";
            Model.Update_area_replace_Id = "something";
            Model.OnSuccessRemoveCallback = "something";

            var result = controller.LoadFiles(files,Model) as ViewResult;
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void tLoadFiles_area_not_this_plugin()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostContentId = 0;
            Model.area = "AnotherPlugin";
            Model.CallbackActionName = "something";
            Model.CallbackControllerName = "something";
            Model.List_content_insert_before_Id = "something";
            Model.Update_area_replace_Id = "something";
            Model.OnSuccessRemoveCallback = "something";
            var result = controller.LoadFiles(files, Model) as ViewResult;
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void tLoadFiles_wrong_route_values()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostContentId = 0;
            Model.area = "PluginImagePostType";
            Model.CallbackActionName = "";
            Model.CallbackControllerName = "something";
            Model.List_content_insert_before_Id = "something";
            Model.Update_area_replace_Id = "something";
            Model.OnSuccessRemoveCallback = "something";
            var result = controller.LoadFiles(files, Model) as ViewResult;
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void tLoadFiles_empty_file_array()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostContentId = 0;
            Model.area = "PluginImagePostType";
            Model.CallbackActionName = "something";
            Model.CallbackControllerName = "something";
            Model.List_content_insert_before_Id = "something";
            Model.Update_area_replace_Id = "something";
            Model.OnSuccessRemoveCallback = "something";
            files = null;
            var result = controller.LoadFiles(files, Model) as ViewResult;
        }

        [TestMethod()]
        public void tLoadFiles_add_new_content()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostContentId = 0;
            Model.area = "PluginImagePostType";
            Model.CallbackActionName = "something";
            Model.CallbackControllerName = "something";
            Model.List_content_insert_before_Id = "something";
            Model.Update_area_replace_Id = "something";
            Model.OnSuccessRemoveCallback = "something";
            
            var result = controller.LoadFiles(files, Model) as ViewResult;

            Assert.AreEqual(3 + files.Count(), _ds.Get().Count());

            IDataStoreRecord newRecord = _ds.Get(0, 1);
            Assert.AreEqual(Model.area, newRecord.ContentPluginName);

            Image newFile_verify = ImageFactory.GetImage(4);
            
            MemoryStream ms = new MemoryStream(newRecord.ContentData);
            Image newFile_Saved = Image.FromStream(ms);
            Assert.AreEqual(newFile_verify, newFile_Saved);

        }

        #endregion

        //#region Modify tests
        //[TestMethod()]
        //public void tModify_input_equals_output()
        //{
        //    VmDisplay input = new VmDisplay();
        //    input.PostContentId = 1;
        //    input.area = "PluginImagePostType";
        //    input.CallbackActionName = "something";
        //    input.CallbackControllerName = "something";
        //    input.List_content_insert_before_Id = "something";
        //    input.Update_area_replace_Id = "something";
        //    input.OnSuccessRemoveCallback = "something";
        //    var result = controller.Modify(input) as ViewResult;
        //    var output = result.Model as VmDisplay;
        //    Assert.AreEqual(input.PostContentId, output.PostContentId);
        //    Assert.AreEqual(input.area, output.area);
        //    Assert.AreEqual(input.CallbackActionName, output.CallbackActionName);
        //    Assert.AreEqual(input.CallbackControllerName, output.CallbackControllerName);
        //    Assert.AreEqual(input.List_content_insert_before_Id, output.List_content_insert_before_Id);
        //    Assert.AreEqual(input.Update_area_replace_Id, output.Update_area_replace_Id);
        //    Assert.AreEqual(input.OnSuccessRemoveCallback, output.OnSuccessRemoveCallback);

        //}

        //[TestMethod()]
        //public void tModify_input_data_equal_output_this_one()
        //{
        //    VmDisplay input = new VmDisplay();
        //    UnicodeEncoding encoding = new UnicodeEncoding();

        //    input.PostContentId = 2;
        //    input.area = "PluginImagePostType";
        //    input.CallbackActionName = "something";
        //    input.CallbackControllerName = "something";
        //    input.List_content_insert_before_Id = "something";
        //    input.Update_area_replace_Id = "something";
        //    input.OnSuccessRemoveCallback = "something";
        //    var result = controller.Modify(input) as ViewResult;
        //    var output = result.Model as VmDisplay;

        //    string imageBase64 = Convert.ToBase64String(_ds.Get(2).ContentData);
        //    var output_string_data = string.Format("data:image/jpeg;base64,{0}", imageBase64);

        //    Assert.AreEqual(output_string_data, output.Data);
        //}

        //[TestMethod()]
        //public void tModifyPost_input_data_equal_output_this_one()
        //{
        //    VmDisplay input = new VmDisplay();
        //    input.PostContentId = 1;
        //    input.area = "PluginImagePostType";
        //    input.CallbackActionName = "something";
        //    input.CallbackControllerName = "something";
        //    input.List_content_insert_before_Id = "something";
        //    input.Update_area_replace_Id = "something";
        //    input.OnSuccessRemoveCallback = "something";
        //    var result = controller.ModifyPost(input) as ViewResult;
        //    var output = result.Model as VmDisplay;
        //    Assert.AreEqual(input.PostContentId, output.PostContentId);
        //    Assert.AreEqual(input.area, output.area);
        //    Assert.AreEqual(input.CallbackActionName, output.CallbackActionName);
        //    Assert.AreEqual(input.CallbackControllerName, output.CallbackControllerName);
        //    Assert.AreEqual(input.List_content_insert_before_Id, output.List_content_insert_before_Id);
        //    Assert.AreEqual(input.Update_area_replace_Id, output.Update_area_replace_Id);
        //    Assert.AreEqual(input.OnSuccessRemoveCallback, output.OnSuccessRemoveCallback);

        //}

        //[TestMethod()]
        //public void tModifyPost_session_exist()
        //{
        //    VmDisplay input = new VmDisplay();
        //    input.PostContentId = 1;
        //    input.area = "PluginImagePostType";
        //    input.CallbackActionName = "something";
        //    input.CallbackControllerName = "something";
        //    input.List_content_insert_before_Id = "something";
        //    input.Update_area_replace_Id = "something";
        //    input.OnSuccessRemoveCallback = "something";
        //    input.Data = "Data";

        //    System.Web.HttpContext.Current = HttpContextFactory.HttpContextCurrent;
        //    var result = controller.ModifyPost(input) as ViewResult;
        //    var session = System.Web.HttpContext.Current.Session["data_store"] as IDataStorePostManage;
        //    Assert.IsNotNull(session);


        //}

        //[TestMethod()]
        //public void tModifyPost_record_is_modified()
        //{
        //    VmDisplay input = new VmDisplay();
        //    input.PostContentId = 1;
        //    input.area = "PluginImagePostType";
        //    input.CallbackActionName = "something";
        //    input.CallbackControllerName = "something";
        //    input.List_content_insert_before_Id = "something";
        //    input.Update_area_replace_Id = "something";
        //    input.OnSuccessRemoveCallback = "something";
        //    input.Data = "Data";

        //    System.Web.HttpContext.Current = HttpContextFactory.HttpContextCurrent;
        //    var result = controller.ModifyPost(input) as ViewResult;
        //    var session = System.Web.HttpContext.Current.Session["data_store"] as IDataStorePostManage;

        //    UnicodeEncoding encoding = new UnicodeEncoding();
        //    Assert.AreEqual(input.Data
        //        , encoding.GetString(session.Get(input.PostContentId).ContentData));

        //}


        //[TestMethod()]
        //public void tModifyPost_record_is_created()
        //{

        //    VmDisplay input = new VmDisplay();
        //    input.PostId = 1;
        //    input.PostContentId = 0;
        //    input.area = "PluginImagePostType";
        //    input.CallbackActionName = "something";
        //    input.CallbackControllerName = "something";
        //    input.List_content_insert_before_Id = "something";
        //    input.Update_area_replace_Id = "something";
        //    input.OnSuccessRemoveCallback = "something";



        //    System.Web.HttpContext.Current = HttpContextFactory.HttpContextCurrent;
        //    var result = controller.ModifyPost(input) as ViewResult;
        //    var session = System.Web.HttpContext.Current.Session["data_store"] as IDataStorePostManage;

        //    Assert.AreEqual(4, session.Get().Count());



        //}

        //[TestMethod()]
        //public void tDeleteContent_by_PostContentId()
        //{
        //    System.Web.HttpContext.Current = HttpContextFactory.HttpContextCurrent;
        //    controller.ControllerContext
        //        = new ControllerContext(new HttpContextWrapper(HttpContextFactory.HttpContextCurrent)
        //                                , new RouteData(), controller);
        //    controller.DeleteContent(2, 0);

        //    dynamic ds = JsonConvert
        //        .DeserializeObject(controller
        //                            .ControllerContext
        //                            .HttpContext
        //                            .Response
        //                            .Output
        //                            .ToString());
        //    Assert.AreEqual(2, (int) ds.PostContentId);
        //    var session = System.Web.HttpContext.Current.Session["data_store"] as IDataStorePostManage;
        //    Assert.AreEqual(2, session.Get().Count());
        //}

        //[TestMethod()]
        //public void tDeleteContent_by_PostContentIdFornewRecords()
        //{
        //    //-------- Step 1 -----------
        //    tModifyPost_record_is_created();
        //    var session = System.Web.HttpContext.Current.Session["data_store"] as IDataStorePostManage;
        //    Assert.AreEqual(4, session.Get().Count());

        //    //-------- Step 2 -----------
        //    System.Web.HttpContext.Current = HttpContextFactory.HttpContextCurrent;
        //    controller.ControllerContext
        //        = new ControllerContext(new HttpContextWrapper(HttpContextFactory.HttpContextCurrent)
        //                                , new RouteData(), controller);
        //    controller.DeleteContent(0,1);

        //    session = System.Web.HttpContext.Current.Session["data_store"] as IDataStorePostManage;

        //    dynamic ds = JsonConvert
        //        .DeserializeObject(controller
        //                            .ControllerContext
        //                            .HttpContext
        //                            .Response
        //                            .Output
        //                            .ToString());

        //    Assert.AreEqual(1, (int)ds.PostContentIdForNewRecords);

        //    Assert.AreEqual(3, session.Get().Count());
        //}
        //#endregion

        [TestMethod()]
        public void tMetadata ()
        {
            AggregateCatalog AggregateCatalog = new AggregateCatalog();
            IEnumerable<AssemblyCatalog> ac = 
                AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(a=>a.FullName.Contains("PluginImagePostType"))
                .Select(a=> new AssemblyCatalog(a));
            foreach (var i in ac)
                AggregateCatalog.Catalogs.Add(i);

            
            CompositionContainer _container = new CompositionContainer(AggregateCatalog);

            var e1 = _container.GetExports<IController, IMetadata>("PluginImagePostType")
                        .Where(m=>m.Metadata.ControllerType== typeof(IPostManager));

            if (e1 != null)
            {
                string Name = e1.Select(m => m.Metadata.Name).SingleOrDefault();
                string Version = e1.Select(m => m.Metadata.Version).SingleOrDefault();
                string ControllerName = e1.Select(m => m.Metadata.ControllerName).SingleOrDefault();
                Type ControllerType = e1.Select(m => m.Metadata.ControllerType).SingleOrDefault();
                string ActionDisplayName = e1.Select(m => m.Metadata.ActionDisplayName).SingleOrDefault();
                string ActionModifyName = e1.Select(m => m.Metadata.ActionModifyName).SingleOrDefault();
                string ActionCreateName = e1.Select(m => m.Metadata.ActionCreateName).SingleOrDefault();
                int prop_count = typeof(IMetadata).GetProperties().Count();

                Assert.AreEqual("PluginImagePostType", Name);
                Assert.AreEqual("1.0",Version);
                Assert.AreEqual("PluginImagePostTypePM", ControllerName);
                Assert.AreEqual(typeof(IPostManager), ControllerType);
                Assert.AreEqual("Display", ActionDisplayName);
                Assert.AreEqual("Modify", ActionModifyName);
                Assert.AreEqual("Create", ActionCreateName);
                Assert.AreEqual(7, prop_count);
            }

            Assert.IsNotNull(e1);
            Assert.AreEqual(1,e1.Count());




        }
    }
}