using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlog.Controllers;
using MyBlog.Infrustructure;
using MyBlog.Tests;
using MyBlog.ViewModels;
using MyBlogContract;
using MyBlogContract.Band;
using NSubstitute;
using PluginTextPostType.Test;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace MyBlog.Controllers.Tests
{
    [TestClass()]
    public class BandControllerTests
    {
       BandController controller;

        [TestInitialize]
        public void Init()
        {
            IUnitOfWork UoW = Substitute.For<IUnitOfWork>();
            DbContextFactory.SetContext();
            var a = DbContextFactory.context;
            ((IUnitOfWork)UoW).db.Returns(a);
            DataStoreBandFactory.SetDataStore();
            controller = new BandController(UoW, DataStoreBandFactory.CurrentDataStore);
        }


 
        [TestMethod()]
        public void ShowContentTest()
        {
            ControllerContextFactory.SetContext();
            controller.ControllerContext = ControllerContextFactory.CurrentContext;
            var result = controller.Index() as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(IList<PostGroupVm>));
            Assert.AreEqual(result.ViewName, "Index");
        }
    }
}