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
        [TestInitialize]
        public void Init()
        {
            var data = new List<ApplicationUser>
            {
                new ApplicationUser ()
            }.AsQueryable();
            var mockSet = new Mock<IDbSet<ApplicationUser>>();
            mockSet.Setup(s => s.Provider).Returns(data.Provider);
            mockSet.Setup(s => s.Expression).Returns(data.Expression);
            mockSet.Setup(s => s.ElementType).Returns(data.ElementType);
            mockSet.Setup(s => s.GetEnumerator()).Returns(data.GetEnumerator());

            var mockIDbContext = new Mock<IDbContext>();
            mockIDbContext.Setup(s => s.Users).Returns(mockSet.Object);
            ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument("DbContext", mockIDbContext.Object);
            
        }
        [TestMethod()]
        public void IndexTest()
        {
            LandingController controller = new LandingController(ninjectKernel.Get<IUnitOfWork>());
            ViewResult result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }


        [TestMethod()]
        public void CreateTest()
        {
            LandingViewModel vm = new LandingViewModel();
            vm.Email = "user.name@domen.ru";
            vm.Password = "123456Sd";
            LandingController controller = new LandingController(ninjectKernel.Get<IUnitOfWork>());
            ActionResult result = controller.Create(vm) as RedirectResult;
            
        }


    }
}