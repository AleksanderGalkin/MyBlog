using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MyBlog.Controllers;
using MyBlog.Infrastructure;
using MyBlog.Infrustructure.Windsor.Interceptors.Audit;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web.Hosting;
using System.Web.Mvc;

//Not used now. Mef is used
namespace MyBlog.Infrustructure.Windsor.Installers
{
    //public class ControllersInstaller : IWindsorInstaller
    //{
    //    static private DirectoryInfo plugins_folder_root_di;
    //    static DirectoryInfo PluginsFolder;
    //    public void Install(IWindsorContainer container, IConfigurationStore store)
    //    {

            //plugins_folder_root_di = new DirectoryInfo(HostingEnvironment.MapPath("~/" + AppSettings.PluginDirectory));
            //PluginsFolder = new DirectoryInfo(HostingEnvironment.MapPath("~/" + AppSettings.PluginDirectory));

            //var plugin_folders = plugins_folder_root_di.GetDirectories();

            
            
            //var classes = plugin_folders.SelectMany(f => f.GetFiles("*.dll"))
            //            .Select(a => AssemblyName.GetAssemblyName(a.FullName))
            //            .Select(an => Assembly.Load(an))
            //            .Select(a => Classes.FromAssembly(a));

            //foreach (var cla in classes)
            //{
            //    container.Register(cla.BasedOn<IController>()
            //        .Configure(c => c.LifestyleTransient()
            //        .Interceptors<LoggerInterceptor>()));
            // }


                

            //container.Register(Classes.FromThisAssembly()
            //    .BasedOn<IController>()
            //    .Configure(c => c.LifestyleTransient()
            //    .Interceptors<LoggerInterceptor>()));

            //container.Register(Classes.FromThisAssembly()
            //    .BasedOn<IInterceptor>());

            //container.Register(Classes.FromThisAssembly()
            //    .BasedOn<IUnitOfWork>()
            //    .WithServiceDefaultInterfaces()
            //    .Configure(c => c.LifestyleTransient()));

            //container.Register(Classes.FromThisAssembly()
            //    .BasedOn<IDbContext>()
            //    .WithServiceDefaultInterfaces()
            //    .Configure(c => c.LifestyleTransient()));

    //    }
    //}
}