using System.Web;
using System.Web.Optimization;

namespace VentaArticulosCredito
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //CSS's utilizados en la vista del Login
            bundles.Add(new StyleBundle("~/Login/Style").Include(
                "~/Content/Semantic UI/css/semantic.min.css",
                "~/Content/Semantic UI/css/animate.css",
                "~/Content/Semantic UI/css/ladda/ladda-themeless.min.css",
                "~/Content/Semantic UI/css/izitoast/iziToast.min.css",
                "~/Content/Semantic UI/css/calendar.min.css",
                "~/Content/Semantic UI/css/login.style.css"
            ));

            //Scripts utilizados en la vista del Login
            bundles.Add(new ScriptBundle("~/Login/Script").Include(
                "~/Content/Scripts/jquery-3.1.1.min.js",
                "~/Content/Semantic UI/js/transition.min.js",
                "~/Content/Semantic UI/js/semantic.min.js",
                "~/Content/Semantic UI/js/ladda/spin.min.js",
                "~/Content/Semantic UI/js/ladda/ladda.min.js",
                "~/Content/Semantic UI/js/izitoast/iziToast.min.js",
                "~/Content/Semantic UI/js/calendar.min.js",
                "~/Content/Scripts/login/login.application.js"
            ));

            //CSS's utilizados en la vista de reset password
            bundles.Add(new StyleBundle("~/Reset/Style").Include(
                "~/Content/Semantic UI/css/semantic.min.css",
                "~/Content/Semantic UI/css/animate.css",
                "~/Content/Semantic UI/css/ladda/ladda-themeless.min.css",
                "~/Content/Semantic UI/css/izitoast/iziToast.min.css",
                "~/Content/Semantic UI/css/calendar.min.css",
                "~/Content/Semantic UI/css/reset.style.css"
            ));

            //Scripts utilizados en la vista de reset password
            bundles.Add(new ScriptBundle("~/Reset/Script").Include(
                "~/Content/Scripts/jquery-1.10.2.min.js",
                "~/Content/Semantic UI/js/transition.min.js",
                "~/Content/Semantic UI/js/semantic.min.js",
                "~/Content/Semantic UI/js/ladda/spin.min.js",
                "~/Content/Semantic UI/js/ladda/ladda.min.js",
                "~/Content/Semantic UI/js/izitoast/iziToast.min.js",
                "~/Content/Semantic UI/js/calendar.min.js",
                "~/Content/Scripts/login/reset.application.js"
            ));
        }
    }
}
