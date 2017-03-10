using NSubstitute;
using PluginImagePostType.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PluginImagePostType.Tests
{
    public class ControllerContextFactory
    {
        private static ControllerContext context;
        public static ControllerContext CurrentContext
        {
            get {
                if (context != null)
                    return context;
                else
                    throw new InvalidOperationException("ControllerContext not available");
            }
        }
        public static void SetContext (ControllerContext Context)
        {
            context = Context;
        }
        public static void SetContext()
        {
            var mockContextBase = Substitute.For<HttpContextBase>();
            var mockControllerContext = Substitute.For<ControllerContext>();
            HttpContextFactory.SetCurrentContext();
            mockContextBase.User.Returns(HttpContextFactory.Current.User);
            mockControllerContext.HttpContext.Returns(mockContextBase);
            context = mockControllerContext;

            var view = Substitute.For<IView>();
            var engine = Substitute.For<IViewEngine>();
            var viewEngineResult = new ViewEngineResult(view, engine);
            engine.FindPartialView(Arg.Any<ControllerContext>(), Arg.Any<string>(), Arg.Any<bool>()).Returns(viewEngineResult);
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(engine);

        }
    }
}


//var homeController = new HomeController();

//var userMock = new Mock<IPrincipal>();
//userMock.Expect(p => p.IsInRole("admin")).Returns(true);

//var contextMock = new Mock<HttpContextBase>();
//contextMock.ExpectGet(ctx => ctx.User)
//                   .Returns(userMock.Object);

//var controllerContextMock = new Mock<ControllerContext>();
//controllerContextMock.ExpectGet(con => con.HttpContext)
//                             .Returns(contextMock.Object);

//homeController.ControllerContext = controllerContextMock.Object;