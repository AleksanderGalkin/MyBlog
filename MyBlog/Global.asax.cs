using Castle.Windsor;
using Castle.Windsor.Installer;
using MyBlog.Infrastructure.Services;
using MyBlog.Infrustructure;
using MyBlog.Infrustructure.Windsor;
using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyBlog
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer container;

        private static void IdContainer()
        {
            container = new WindsorContainer().Install(FromAssembly.This());

            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();
            AutoMapperConfig.RegisterMappings();
          //  MvcApplication.IdContainer();
            //    ModelBinders.Binders.Add(typeof(PostEditVm<IContentType>), new PostEditVmModelBinder2());
            //  ModelBinders.Binders.Add(typeof(IContentType), new IContentTypeBinder());
            //  ModelBinders.Binders.Add(typeof(PostEditVm), new PostEditVmBinder());

            ControllerBuilder.Current.SetControllerFactory(new CustomControllerFactory());
            PlugInFactory.InitFactory();
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new CustomViewEngine(PlugInFactory.GetPluginNamesList()));
            ModelBinders.Binders.DefaultBinder = new DemBinder();

        }

        protected void Application_End()
        {
            container.Dispose();
        }
    }
}
