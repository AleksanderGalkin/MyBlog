using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MyBlog.Controllers;
using MyBlog.Infrustructure.Windsor.Interceptors.Audit;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web.Mvc;

namespace MyBlog.Infrustructure.Windsor.Installers
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                .BasedOn<IController>()
                 .Configure(c => c.LifestyleTransient()
                .Interceptors<LoggerInterceptor>()));

            container.Register(Classes.FromThisAssembly()
                .BasedOn<IInterceptor>());

            container.Register(Classes.FromThisAssembly()
                .BasedOn<IUnitOfWork>()
                .WithServiceDefaultInterfaces()
                .Configure(c => c.LifestyleTransient()));

            container.Register(Classes.FromThisAssembly()
                .BasedOn<IDbContext>()
                .WithServiceDefaultInterfaces()
                .Configure(c => c.LifestyleTransient()));

        }
    }
}