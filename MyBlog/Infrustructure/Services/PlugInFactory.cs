﻿using MyBlog.Infrastructure;
using MyBlogContract;
using MyBlogContract.Band;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MyBlog.Infrastructure.Services
{
    
    public static class PlugInFactory
    {
        private static AggregateCatalog AggregateCatalog;
        private static  CompositionContainer _container;
        private static DirectoryInfo plugins_folder_root_di;
        private static IDictionary<IMetadata,Type> types_of_controllers;

        private static IDataStoreBand _ds;


        static private IEnumerable<IPlugIn> plugin_list { get; set; }

        static PlugInFactory()
        {
            plugins_folder_root_di = new DirectoryInfo(HostingEnvironment.MapPath("~/" + AppSettings.PluginDirectory));
            types_of_controllers = new Dictionary<IMetadata, Type>();
        }

        public static void InitFactory()
        {

            AssemblyCatalog AssemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var plugin_folders = plugins_folder_root_di.GetDirectories();
            AggregateCatalog = new AggregateCatalog( plugin_folders.Select(di=>new DirectoryCatalog(di.FullName)));
            AggregateCatalog.Catalogs.Add(AssemblyCatalog);
            _container = new CompositionContainer(AggregateCatalog);
            plugin_list = _container.GetExportedValues<IPlugIn>();
            _ds = new DataStore();

            

        }
      

        static public List<string> GetPluginNamesList ()
        {
            List<string> list = new List<string>();
            foreach(var p in plugin_list)
            {
                list.Add(p.PluginName);
            }
            return list;
        }

        public static IController GetController (string ControllerName, string Plugin)
        {
            IController controller = null;


            var e1 = _container.GetExports<IController, IMetadata>(Plugin);
            var e2 = e1.Where(e => e.Metadata.ControllerName.Equals(ControllerName));
            var export = e2.SingleOrDefault();
            if (export != null)
            {
                controller = export.Value as IController;

            }

            //if (controller == null)
            //{
            //    var export_not_plugin = _container.GetExports<IController, IMetadata>()
            //        .Where(e => e.Metadata.ControllerName.Equals(ControllerName))
            //        .SingleOrDefault();
            //    if (export_not_plugin != null)
            //    {
            //        controller = export_not_plugin.Value as IController;
            //    }
            //}

            return controller;
        }

        public static object GetModelByInterface(Type Interface, string Plugin)
        {
            object obj = null;
            var e1 = _container.GetExports(Interface,null,Plugin);
            var export = e1.SingleOrDefault();
            if (export != null)
            {
                obj = export.Value;

            }
            return obj;
        }

        //public static IController GetControllerInstance<T>(string Plugin)
        //{
        //    IController Controller 
        //        = (IController)_container.GetExportedValueOrDefault<T>(Plugin);
            
        //    return Controller;
        //}

        public static string GetControllerNameByInterface(Type type, string Plugin)
        {
            var meta = _container.GetExports<IController, IMetadata>(Plugin)
               .Where(m=>m.Metadata.ControllerType == type)
               .SingleOrDefault();

            var name = meta.Metadata.ControllerName;
            return name;
        }

        public static string GetActionDisplayNameByInterface(Type type, string Plugin)
        {
            var meta = _container.GetExports<IController, IMetadata>(Plugin)
                .Where(m => m.Metadata.ControllerType == type)
                .SingleOrDefault();
            var name = meta.Metadata.ActionDisplayName;

            return name;
        }

        public static string GetActionModifyNameByInterface(Type type, string Plugin)
        {
            var meta = _container.GetExports<IController, IMetadata>(Plugin)
               .Where(m => m.Metadata.ControllerType == type)
               .SingleOrDefault();
            var name = meta.Metadata.ActionModifyName;

            return name;
        }
    }
}