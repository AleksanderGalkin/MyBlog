using System.Web;
using System.Web.Optimization;

namespace MyBlog
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                       "~/Scripts/jquery-ui-{version}.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/ajax").Include(
                       "~/Scripts/jquery.unobtrusive-ajax.min_.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquerymaskedinput").Include(
                        "~/Scripts/jquery.maskedinput.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/_modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/umd/popper.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/locales/bootstrap-datepicker.ru.min.js",
                      "~/Scripts/bootstrap-modal.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/tinymce").Include(
                      "~/Scripts/tinymce/tinymce.min.js",
                      "~/Scripts/tinymce/prism.js"));

            bundles.Add(new ScriptBundle("~/bundles/prism").Include(
                           "~/Scripts/tinymce/prism.js"));

            bundles.Add(new StyleBundle("~/Content/woothemes-FlexSlider").Include(
                      "~/woothemes-FlexSlider/flexslider.css"));
            bundles.Add(new ScriptBundle("~/bundles/woothemes-FlexSlider").Include(
            "~/woothemes-FlexSlider/jquery.flexslider-min.js"));

            bundles.Add(new ScriptBundle("~/bundles/fileupload").Include(
                      "~/Backload/Client/blueimp/fileupload/js/vendor/jquery.ui.widget.js",
//                      "~/Backload/Client/blueimp/templates/js/tmpl.min.js",
                      
                      "~/Backload/Client/blueimp/loadimage/js/load-image.all.min.js",
                      "~/Backload/Client/blueimp/blob/js/canvas-to-blob.min.js",
                      "~/Scripts/bootstrap.min.js",

                      "~/Backload/Client/blueimp/fileupload/js/jquery.iframe-transport.js",
                      "~/Backload/Client/blueimp/fileupload/js/jquery.fileupload.js",
                      "~/Backload/Client/blueimp/fileupload/js/jquery.fileupload-process.js",
                      "~/Backload/Client/blueimp/fileupload/js/jquery.fileupload-image.js",
                      "~/Backload/Client/blueimp/fileupload/js/jquery.fileupload-validate.js"
                      //"~/Backload/Client/blueimp/fileupload/js/jquery.fileupload-ui.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-datepicker.css",
                      "~/Content/site.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/landing").Include(
                      "~/Content/landing.css"));

            bundles.Add(new StyleBundle("~/Content/fileupload").Include(
                     "~/Backload/Client/blueimp/fileupload/css/jquery.fileupload.css",
                     "~/Content/bootstrap.min.css"
                     ));

            bundles.Add(new StyleBundle("~/Content/prism").Include(
                     "~/Scripts/tinymce/prism.css"
                     ));

        }
    }
}
