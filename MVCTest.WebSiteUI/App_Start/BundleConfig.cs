using System.Web;
using System.Web.Optimization;

namespace MVCTest.WebSiteUI
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                        "~/Scripts/Ajax.js",
                        "~/Scripts/customNotice.js",
                        "~/Scripts/Common.js"));


            bundles.Add(new ScriptBundle("~/bundles/contact").Include(                        
                        "~/Scripts/Contact.js"));

            bundles.Add(new ScriptBundle("~/bundles/account").Include(
                        "~/Scripts/Account.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootbox.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/animate.css",
                      "~/Content/customNotice.css",
                      "~/Content/customAlert.css",
                      "~/Content/glyphicon-animations.css",
                      "~/Content/site.css"));
        }
    }
}
