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
using System.Threading.Tasks;
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
            InitController controller = new InitController(ninjectKernel.Get<IUnitOfWork>());
            ViewResult result = controller.Welcome() as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Index",result.ViewName);
        }


        [TestMethod()]
        public void RegisterPost_NotValidModel()
        {
            RegisterVm vm = new RegisterVm();
            vm.FullName = "Joe Doe";
            vm.EmailReg = "Joe.Doe@unknown.no";
            InitController controller = new InitController(ninjectKernel.Get<IUnitOfWork>());
            controller.ModelState.AddModelError("test","Test Model Error");
            Task<ActionResult> taskResult = controller.Register(vm) as Task<ActionResult>;
            ViewResult result = taskResult.Result as ViewResult;
            Assert.AreEqual(result.ViewName,"Index");
            Assert.IsInstanceOfType(result.Model,typeof(Tuple<RegisterVm, LoginVm>));
            Assert.AreEqual((result.Model as Tuple<RegisterVm, LoginVm>).Item1.FullName, vm.FullName);
            Assert.AreEqual((result.Model as Tuple<RegisterVm, LoginVm>).Item1.EmailReg, vm.EmailReg);
        }

        [TestMethod()]
        public void RegisterPost_Valid()
        {
            RegisterVm vm = new RegisterVm();
            vm.FullName = "Joe Doe";
            vm.EmailReg = "Joe.Doe@unknown.no";
        //    Mock < UserManager < IdentityUser >> = new Mock<ApplicationUserManager>();
            InitController controller = new InitController(ninjectKernel.Get<IUnitOfWork>());
            Task<ActionResult> taskResult = controller.Register(vm) as Task<ActionResult>;
            //mockSet.Verify(x => x.Add(It.IsAny<ApplicationUser>()));
            RedirectToRouteResult result = taskResult.Result as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["action"], "Index");
        }

    }
}