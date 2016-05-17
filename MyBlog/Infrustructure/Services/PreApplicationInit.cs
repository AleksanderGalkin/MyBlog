using System.Linq;
using System.Web;
using System.IO;
using System.Web.Hosting;
using System.Web.Compilation;
using System.Reflection;
using System;
using MainApp.Infrastructure.Services;
using MyBlog.Infrastructure;

[assembly: PreApplicationStartMethod(typeof(PreApplicationInit), "Initialize")]
namespace MainApp.Infrastructure.Services
{
    public static class PreApplicationInit
    {

        static PreApplicationInit()
        {
            plugins_folder_root_di = new DirectoryInfo(HostingEnvironment.MapPath("~/"+ AppSettings.PluginDirectory));
            PluginsFolder = new DirectoryInfo(HostingEnvironment.MapPath("~/" + AppSettings.PluginDirectory));
        }

        /// <summary>
        /// The source plugin folder from which to shadow copy from
        /// </summary>
        /// <remarks>
        /// This folder can contain sub folderst to organize plugin types
        /// </remarks>
        static private DirectoryInfo plugins_folder_root_di;
        static DirectoryInfo PluginsFolder;
        public static void Initialize()
        {
            

            string backup = AppDomain.CurrentDomain.SetupInformation.ShadowCopyDirectories;

            AppDomain.CurrentDomain.SetShadowCopyPath(AppDomain.CurrentDomain.SetupInformation.ShadowCopyDirectories +
                Path.PathSeparator + PluginsFolder.FullName);

            AppDomain.CurrentDomain.AppendPrivatePath(PluginsFolder.FullName);

            AppDomain.CurrentDomain.SetShadowCopyFiles();

            var plugin_folders = plugins_folder_root_di.GetDirectories();
            foreach (var folder in plugin_folders)
            {
                var dlls = folder.GetFiles("*.dll");
                var assembly_names = dlls.Select(f => AssemblyName.GetAssemblyName(f.FullName));
                var a = assembly_names.Select(an => AppDomain.CurrentDomain.Load(an));
                foreach (var r in a)
                {
                    BuildManager.AddReferencedAssembly(r);
                }
            }

        }
    }
}