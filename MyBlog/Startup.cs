using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyBlog.Startup))]
//[assembly: Log(AttributeTargetTypes = "MyBlog.Controllers.*", ApplyToStateMachine = false,AspectPriority =1)]
namespace MyBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}


