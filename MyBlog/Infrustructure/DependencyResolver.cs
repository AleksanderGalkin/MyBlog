using Microsoft.AspNet.Identity;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Infrustructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<IDbContext>().To<Models.ApplicationDbContext>();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            //kernel.Bind<IUserStore<ApplicationUser>>().To<UserManager<ApplicationUser>>();
            //kernel.Bind<IUnitOfWork>().To<UnitOfWork>();

        }
    }

}