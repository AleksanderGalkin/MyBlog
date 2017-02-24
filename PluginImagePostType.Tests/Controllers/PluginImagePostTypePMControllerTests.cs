using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlogContract;
using MyBlogContract.PostManage;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using PluginImagePostType.Models;
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
            DataStorePostManageFactory.SetDataStore();
            _ds = DataStorePostManageFactory.CurrentDataStore;

            HttpPostedFileBase hpfb = Substitute.For<HttpPostedFileBase>();
            hpfb.FileName.Returns("File1.bmp");
            Image newFile = ImageFactory.GetImage(4);
            MemoryStream ms2 = new MemoryStream();
            newFile.Save(ms2, ImageFormat.Bmp);
            hpfb.InputStream.Returns(ms2);
            hpfb.ContentLength.Returns((int)ms2.Length);
            hpfb.ContentType.Returns("image/bmp");
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
            Model.AreaName = "PluginImagePostType";
            var result = controller.Display(Model) as ViewResult;
            var output_model = result.Model as VmDisplay;
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(1, output_model.PostId);
            Assert.AreEqual(2, output_model.PostContentId);
            Assert.AreEqual("PluginImagePostType", output_model.AreaName);
            Assert.AreEqual("AnyComment2", output_model.Comment);

            string imageBase64 = Convert.ToBase64String(_ds.GetContent(2).ContentData);
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
            Model.AreaName = "PluginImagePostType";

            var result = controller.Display(Model) as ViewResult;
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void tDisplay_PostContentId_not_belong_postid()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostId = 1;
            Model.PostContentId = 3;
            Model.AreaName = "PluginImagePostType";
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
        [ExpectedException(typeof(InvalidOperationException))]
        public void tCreate_wrong_PostContentId()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostContentId = 1;
            Model.AreaName = "PluginImagePostType";
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
            Model.AreaName = "PluginImagePostType";
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
            Model.AreaName = "PluginImagePostType";
            Model.CallbackActionName = "something";
            Model.CallbackControllerName = "something";
            Model.List_content_insert_before_Id = "something";
            Model.Update_area_replace_Id = "something";
            Model.OnSuccessRemoveCallback = "something";

            var result = controller.LoadFiles(files, Model) as ViewResult;
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void tLoadFiles_area_not_this_plugin()
        {
            IDEModelPostManage Model = Substitute.For<IDEModelPostManage>();
            Model.PostContentId = 0;
            Model.AreaName = "AnotherPlugin";
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
            Model.AreaName = "PluginImagePostType";
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
            Model.AreaName = "PluginImagePostType";
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
            Model.AreaName = "PluginImagePostType";
            Model.CallbackActionName = "something";
            Model.CallbackControllerName = "something";
            Model.List_content_insert_before_Id = "something";
            Model.Update_area_replace_Id = "something";
            Model.OnSuccessRemoveCallback = "something";

            var result = controller.LoadFiles(files, Model) as ViewResult;

            _ds.Received().Modify(Arg.Any<IDataStoreRecord>());
            _ds.Received().Modify(Arg.Is<IDataStoreRecord>(x => checkModifyArgs(x)));

        }

        private static bool checkModifyArgs(IDataStoreRecord arg)
        {
            Image newFile_expected = ImageFactory.GetImage(4);
            MemoryStream ms_expected = new MemoryStream();
            newFile_expected.Save(ms_expected, ImageFormat.Bmp);
            string ms_expected_string = Convert.ToBase64String(ms_expected.ToArray());

            MemoryStream ms_actual = new MemoryStream(arg.ContentData);
            string ms_actual_string = Convert.ToBase64String(ms_actual.ToArray());

            return ms_expected_string == ms_actual_string;
        }
        #endregion

        //#region Modify tests
        [TestMethod()]
        public void tModify_input_equals_output()
        {
            VmDisplay input = new VmDisplay();
            input.PostContentId = 1;
            input.AreaName = "PluginImagePostType";
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

            input.PostContentId = 2;
            input.AreaName = "PluginImagePostType";
            input.CallbackActionName = "something";
            input.CallbackControllerName = "something";
            input.List_content_insert_before_Id = "something";
            input.Update_area_replace_Id = "something";
            input.OnSuccessRemoveCallback = "something";
            var result = controller.Modify(input) as ViewResult;
            var output = result.Model as VmDisplay;

            string imageBase64 = Convert.ToBase64String(_ds.GetContent(2).ContentData);
            var output_string_data = string.Format("data:image/jpeg;base64,{0}", imageBase64);

            Assert.AreEqual(output_string_data, output.Data);
        }

        [TestMethod()]
        public void tModifyPost_input_data_equal_output_this_one()
        {
            VmDisplay input = new VmDisplay();
            input.PostContentId = 1;
            input.AreaName = "PluginImagePostType";
            input.CallbackActionName = "something";
            input.CallbackControllerName = "something";
            input.List_content_insert_before_Id = "something";
            input.Update_area_replace_Id = "something";
            input.OnSuccessRemoveCallback = "something";
            var result = controller.ModifyPost(input) as ViewResult;
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
            input.AreaName = "PluginImagePostType";
            input.CallbackActionName = "something";
            input.CallbackControllerName = "something";
            input.List_content_insert_before_Id = "something";
            input.Update_area_replace_Id = "something";
            input.OnSuccessRemoveCallback = "something";
            input.Data = "Data";

            var result = controller.ModifyPost(input) as ViewResult;
            int cntContents = _ds.GetAllContents().Where(x => x.PostContentId == input.PostContentId).Count();
            Assert.IsTrue(cntContents > 0,"Session not contains records");


        }



        [TestMethod()]
        public void tMetadata()
        {
            AggregateCatalog AggregateCatalog = new AggregateCatalog();
            IEnumerable<AssemblyCatalog> ac =
                AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(a => a.FullName.Contains("PluginImagePostType"))
                .Select(a => new AssemblyCatalog(a));
            foreach (var i in ac)
                AggregateCatalog.Catalogs.Add(i);


            CompositionContainer _container = new CompositionContainer(AggregateCatalog);

            var e1 = _container.GetExports<IController, IMetadata>("PluginImagePostType")
                        .Where(m => m.Metadata.ControllerType == typeof(IPostManager));

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
                Assert.AreEqual("1.0", Version);
                Assert.AreEqual("PluginImagePostTypePM", ControllerName);
                Assert.AreEqual(typeof(IPostManager), ControllerType);
                Assert.AreEqual("Display", ActionDisplayName);
                Assert.AreEqual("Modify", ActionModifyName);
                Assert.AreEqual("Create", ActionCreateName);
                Assert.AreEqual(7, prop_count);
            }

            Assert.IsNotNull(e1);
            Assert.AreEqual(1, e1.Count());




        }
    }
}