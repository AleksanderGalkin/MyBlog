using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyBlog.App_Start;
using MyBlog.Controllers;
using MyBlog.Infrustructure;
using MyBlog.Models;
using MyBlog.ViewModels;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MyBlog.Controllers.Tests
{
    [TestClass()]
    public class LandingControllerTests
    {
        IKernel ninjectKernel = new StandardKernel();
        Mock<IDbContext> mockIDbContext;
        Mock<IDbSet<ApplicationUser>> mockSet;
        [TestInitialize]
        public void Init()
        {
            List<ApplicationUser> data = new List<ApplicationUser>
            {
                new ApplicationUser ()
            };
            IQueryable<ApplicationUser> qdata  = data.AsQueryable();
            mockSet = new Mock<IDbSet<ApplicationUser>>();
            setDataForIDbSet(qdata);
            mockIDbContext = new Mock<IDbContext>();
            mockIDbContext.Setup(s => s.Users).Returns(mockSet.Object);
            ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument("DbContext", mockIDbContext.Object);
            
        }

        private void setDataForIDbSet (IQueryable<ApplicationUser> qdata)
        {
            mockSet.Setup(s => s.Provider).Returns(qdata.Provider);
            mockSet.Setup(s => s.Expression).Returns(qdata.Expression);
            mockSet.Setup(s => s.ElementType).Returns(qdata.ElementType);
            mockSet.Setup(s => s.GetEnumerator()).Returns(qdata.GetEnumerator());
        }
        [TestMethod()]
        public void Index()
        {
            LandingController controller = new LandingController(ninjectKernel.Get<IUnitOfWork>());
            ViewResult result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Index",result.ViewName);
        }


        [TestMethod()]
        public void CreatePost_NotValidModel()
        {
            LandingViewModel vm = new LandingViewModel();
            vm.Email = "name@domen";
            vm.Password = "1212";
            LandingController controller = new LandingController(ninjectKernel.Get<IUnitOfWork>());
            controller.ModelState.AddModelError("test","Test Model Error");
            ViewResult result = controller.Create(vm) as ViewResult;
            Assert.AreEqual(result.ViewName,"Index");
            Assert.IsInstanceOfType(result.Model,typeof(LandingViewModel));
            Assert.AreEqual((result.Model as LandingViewModel).Email, vm.Email);
            Assert.AreEqual((result.Model as LandingViewModel).Password, vm.Password);
        }

        [TestMethod()]
        public void CreatePost_Valid()
        {
            LandingViewModel vm = new LandingViewModel();
            LandingController controller = new LandingController(ninjectKernel.Get<IUnitOfWork>());
            RedirectToRouteResult result = controller.Create(vm) as RedirectToRouteResult;
            mockSet.Verify(x => x.Add(It.IsAny<ApplicationUser>()));
            Assert.AreEqual(result.RouteValues["action"], "Index");
        }

    }
}