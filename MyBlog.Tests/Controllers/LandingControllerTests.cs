using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyBlog.Controllers;
using MyBlog.Infrustructure;
using MyBlog.Infrustructure.Windsor;
using MyBlog.Models;
using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyBlog.Tests.Controllers
{
    [TestClass()]
    public class LandingControllerTests
    {
        private static IWindsorContainer container;
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
          //  mockIDbContext.Setup(s => s.Users).Returns(mockSet.Object);

            container = new WindsorContainer().Install(FromAssembly.InThisApplication());
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

        }

        private void setDataForIDbSet (IQueryable<ApplicationUser> qdata)
        {
            mockSet.Setup(s => s.Provider).Returns(qdata.Provider);
            mockSet.Setup(s => s.Expression).Returns(qdata.Expression);
            mockSet.Setup(s => s.ElementType).Returns(qdata.ElementType);
            mockSet.Setup(s => s.GetEnumerator()).Returns(qdata.GetEnumerator());
        }
        [TestMethod()]
        public void Welcome()
        {
            InitController controller = new InitController(container.Resolve<IUnitOfWork>());
            ViewResult result = controller.Welcome() as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Welcome",result.ViewName);
        }


        [TestMethod()]
        public void RegisterPost_NotValidModel()
        {
            RegisterVm vm = new RegisterVm();
            vm.FullName = "Joe Doe";
            vm.EmailReg = "Joe.Doe@unknown.no";
            InitController controller = new InitController(container.Resolve<IUnitOfWork>());
            controller.ModelState.AddModelError("test","Test Model Error");
            Task<ActionResult> taskResult = controller.Register(vm) as Task<ActionResult>;
            ViewResult result = taskResult.Result as ViewResult;
            Assert.AreEqual(result.ViewName,"Index");
            Assert.IsInstanceOfType(result.Model,typeof(Tuple<RegisterVm, LoginVm>));
            Assert.AreEqual((result.Model as Tuple<RegisterVm, LoginVm>).Item1.FullName, vm.FullName);
            Assert.AreEqual((result.Model as Tuple<RegisterVm, LoginVm>).Item1.EmailReg, vm.EmailReg);
        }

    }
}