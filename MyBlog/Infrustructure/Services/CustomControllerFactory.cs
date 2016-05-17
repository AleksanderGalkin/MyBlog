using MainApp.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace MyBlog.Infrastructure.Services
{
    public class CustomControllerFactory : IControllerFactory
    {
        private readonly DefaultControllerFactory _defaultControllerFactory;

        public CustomControllerFactory()
        {
            _defaultControllerFactory = new DefaultControllerFactory();
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            string plugin_name = requestContext.RouteData.DataTokens["area"] as string;
            var controller = PlugInFactory
                .GetController(controllerName, plugin_name);

            if (controller == null)
                controller = _defaultControllerFactory.CreateController(requestContext, controllerName);
            if (controller == null)
                throw new Exception("Controller not found!");

            return controller;
            
        }



        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }

        public void ReleaseController(IController controller)
        {
            var disposableController = controller as IDisposable;

            if (disposableController != null)
            {
                disposableController.Dispose();
            }
        }

    }
}