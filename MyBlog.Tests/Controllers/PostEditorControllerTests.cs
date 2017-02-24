using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlog.Controllers;
using MyBlog.Infrustructure;
using MyBlog.Tests;
using MyBlog.ViewModels;
using MyBlogContract;
using NSubstitute;
using PluginTextPostType.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MyBlog.Infrastructure.Services;
using MyBlog.Models;
using MyBlog.Infrustructure.Sevices;

namespace MyBlog.Controllers.Tests
{
    [TestClass()]
    public class PostEditorControllerTests
    {
        private PostEditorController controller;
        private IUnitOfWork UoW;
        [TestInitialize]
        public void Init()
        {
            UoW = Substitute.For<IUnitOfWork>();
            DbContextFactory.SetContext();
            var mockEmptyDbContext = DbContextFactory.context;

            DbContextFillFactory.SetDbContext(mockEmptyDbContext);
            DbContextFillFactory.SetPostData();
            var mockFillDbContext = DbContextFillFactory.CurrentContext;

            ((IUnitOfWork)UoW).db.Returns(mockFillDbContext);
            DataStorePostManageFactory.SetDataStore();
            controller = new PostEditorController(UoW, DataStorePostManageFactory.CurrentDataStore);
            AutoMapperConfig.RegisterMappings();
        }

        [TestMethod()]
        public void CreatePostTest()
        {
            ControllerContextFactory.SetContext();
            controller.ControllerContext = ControllerContextFactory.CurrentContext;
             
            IList<IDataStoreRecord> session_list = new List<IDataStoreRecord>();
            IDataStoreRecord session_item = Substitute.For<IDataStoreRecord>();
            session_list.Add(session_item);

            controller.ControllerContext.HttpContext.Session["PostContents"] = session_list;

            var result = controller.CreatePost("PluginTextPostType") as ViewResult;

            Assert.IsInstanceOfType(result.Model, typeof(PostVm));
            Assert.AreEqual( 1, (result.Model as PostVm).PostContents.Count);
            Assert.AreEqual(result.ViewName, "EditPost");
        }

        [TestMethod()]
        public void EditPostTest()
        {
            var result = controller.EditPost(1/*PostId*/) as ViewResult;

            Assert.IsInstanceOfType(result.Model, typeof(PostVm));
            Assert.AreEqual( 3, (result.Model as PostVm).PostContents.Count);
            Assert.AreEqual(1, (result.Model as PostVm).PostContents.Where(x=>x.PostContentId==1).Select(x=>x.PostContentId).SingleOrDefault());
            Assert.AreEqual(result.ViewName, "EditPost");
        }

        [TestMethod()]
        public void EditPostTest1_AddNewPost()
        {
            ControllerContextFactory.SetContext();
            controller.ControllerContext = ControllerContextFactory.CurrentContext;
            PostVm mockPostVm = Substitute.For<PostVm>();
            PostVm mockPostVm2 = Substitute.For<PostVm>();
            mockPostVm.PostId=0;
            var cntDbPosts_Before = UoW.db.Posts.Count();

            controller.EditPost(mockPostVm);
            var cntDbPosts_After = UoW.db.Posts.Count();

            Assert.AreEqual(cntDbPosts_Before + 1, cntDbPosts_After,"Posts count is wrong");

        }

        [TestMethod()]
        public void EditPostTest1_EditExistPost()
        {
            ControllerContextFactory.SetContext();
            controller.ControllerContext = ControllerContextFactory.CurrentContext;
            PostVm PostVm = new PostVm();
            PostVm.PostId = 1;
            PostVm.Tittle = "Post1Editable";
            DataStoreRecord PostContent =  new DataStoreRecord();
            PostContent.PostId = 1;
            PostContent.PostContentId = 2;
            PostContent.Comment = "PostConten1Editable";
            PostVm.PostContents = new List<IDataStoreRecord>();
            PostVm.PostContents.Add(PostContent);
            

            controller.EditPost(PostVm);
            Post p = UoW.db.Posts.ElementAt(0);

            Assert.AreEqual("Post1Editable", p.Tittle, "Post was not editable");


        }

        [TestMethod()]
        public void DeletePostTest()
        {
            var result = controller.DeletePost(1/*PostId*/) as ViewResult;

            Assert.IsInstanceOfType(result.Model, typeof(PostVm));
            Assert.AreEqual( 1, (result.Model as PostVm).PostId, "PostId not match");
            Assert.AreEqual("DeletePost", result.ViewName, "ViewName is wrong");
            
        }

        [TestMethod()]
        public void DeletePostTest1()
        {
            PostVm PostVm = new PostVm();
            PostVm.PostId = 1;

            var result = controller.DeletePost(PostVm) as RedirectToRouteResult;

            Assert.AreEqual(0, UoW.db.Posts.Count());
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("Band", result.RouteValues["Controller"]);
        }

        [TestMethod()]
        public void CancelPostEditionTest()
        {
            var result = controller.CancelPostEdition() as RedirectToRouteResult;
            Assert.AreEqual(0, DataStorePostManageFactory.CurrentDataStore.GetAllContents().Count());
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("Band", result.RouteValues["Controller"]);
        }
    }
}