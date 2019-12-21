using System.Web.Optimization;

namespace OneTech
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/mainJs").Include(
                "~/Scripts/main.js"));
            // bootstrap select
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-select-js").Include(
                "~/Scripts/bootstrap-select.min.js"));



            bundles.Add(new ScriptBundle("~/bundles/root-script").Include(
                "~/Content/vendors/js/vendor.bundle.base.js",
                "~/Content/vendors/chart.js/Chart.min.js",
                "~/Scripts/js/off-canvas.js", "~/Scripts/js/hoverable-collapse.js",
                "~/Scripts/js/misc.js", "~/Scripts/js/dashboard.js", "~/Scripts/js/todolist.js"));
        }
    }
}
