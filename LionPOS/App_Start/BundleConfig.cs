using System.Web;
using System.Web.Optimization;

namespace LionPOS
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
           // bundles.Add(new ScriptBundle("~/bundles/jquery/Login").Include(


           //"~/Scripts/jquery-{version}.js",
           //  "~/Content/assets/globals/plugins/modernizr/modernizr.min.js",
           //  "~/Content/assets/globals/js/global-vendors.js",
           //  "~/Content/assets/globals/scripts/user-pages.js",
           //  "~/Content/assets/globals/js/pleasure.js",
           //  "~/Content/assets/admin1/js/layout.js"


           //         ));


           // bundles.Add(new StyleBundle("~/bundles/css/Login").Include(

           //   "~/Content/assets/admin1/css/admin1.css",
           //   "~/Content/assets/globals/css/elements.css",
           //   "~/Content/assets/globals/plugins/bootstrap-social/bootstrap-social.css",
           //   "~/Content/assets/globals/css/plugins.css"

           //      ));




            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.unobtrusive*",
                "~/Content/assets/globals/plugins/modernizr/modernizr.min.js",
                         "~/Content/assets/globals/js/global-vendors.js",
                         //"~/Content/assets/globals/plugins/gmaps/gmaps.js",
                         //"~/Content/assets/globals/plugins/bxslider/jquery.bxslider.min.js",
                         //"~/Content/assets/globals/plugins/audiojs/audiojs/audio.min.js",
                         //"~/Content/assets/globals/plugins/d3/d3.min.js",
                         //"~/Content/assets/globals/plugins/rickshaw/rickshaw.min.js",
                         //"~/Content/assets/globals/plugins/jquery-knob/excanvas.js",
                         //"~/Content/assets/globals/plugins/jquery-knob/dist/jquery.knob.min.js",
                         //"~/Content/assets/globals/plugins/gauge/gauge.min.js",
                         "~/Content/assets/globals/js/pleasure.js",
                         "~/Content/assets/admin1/js/layout.js",
                        // "~/Content/assets/globals/scripts/sliders.js",
                        // "~/Content/assets/globals/scripts/maps-google.js",
                        // "~/Content/assets/globals/scripts/widget-audio.js",
                        // "~/Content/assets/globals/scripts/charts-knob.js",
                        // "~/Content/assets/globals/scripts/index.js",
                        //"~/Content/assets/globals/plugins/media/js/jquery.dataTables.min.js",
                        //"~/Content/assets/globals/plugins/themes/bootstrap/dataTables.bootstrap.js",
                        //"~/Content/assets/globals/scripts/tables-datatables.js",
                        "~/Scripts/jquery-ui.js"


                     ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryForm2").Include(
                              "~/Scripts/jquery-{version}.js",
              "~/Scripts/jquery-ui-{version}.js",
              "~/Scripts/jquery.unobtrusive*",
         "~/Content/assets/globals/plugins/modernizr/modernizr.min.js",
               "~/Content/assets/globals/js/global-vendors.js",
               "~/Content/assets/globals/plugins/bxslider/jquery.bxslider.min.js",
               "~/Content/assets/globals/plugins/audiojs/audiojs/audio.min.js",
               "~/Content/assets/globals/plugins/d3/d3.min.js",
               "~/Content/assets/globals/plugins/rickshaw/rickshaw.min.js",
               "~/Content/assets/globals/plugins/jquery-knob/excanvas.js",
               "~/Content/assets/globals/plugins/jquery-knob/dist/jquery.knob.min.js",
               "~/Content/assets/globals/plugins/gauge/gauge.min.js",
               "~/Content/assets/globals/js/pleasure.js",
               "~/Content/assets/admin1/js/layout.js",
               "~/Content/assets/globals/scripts/sliders.js",
               "~/Content/assets/globals/scripts/index.js",
              "~/Content/assets/globals/plugins/media/js/jquery.dataTables.min.js",
              "~/Content/assets/globals/plugins/themes/bootstrap/dataTables.bootstrap.js",
              "~/Content/assets/globals/plugins/chosen/chosen.jquery.min.js",
              "~/Content/assets/globals/scripts/tables-datatables.js",
              "~/Scripts/jquery-ui.js"


           ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryForms").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.unobtrusive*",

                        "~/Content/assets/globals/plugins/modernizr/modernizr.min.js",
                       "~/Content/assets/globals/js/global-vendors.js",
                                                                                    "~/Content/assets/globals/plugins/gmaps/gmaps.js",
                         "~/Content/assets/globals/plugins/bxslider/jquery.bxslider.min.js",
                         "~/Content/assets/globals/plugins/audiojs/audiojs/audio.min.js",
                         "~/Content/assets/globals/plugins/d3/d3.min.js",
                         "~/Content/assets/globals/plugins/rickshaw/rickshaw.min.js",
                         "~/Content/assets/globals/plugins/jquery-knob/excanvas.js",
                         "~/Content/assets/globals/plugins/jquery-knob/dist/jquery.knob.min.js",
                         "~/Content/assets/globals/plugins/gauge/gauge.min.js",
                       "~/Content/assets/globals/js/pleasure.js",
                       "~/Content/assets/admin1/js/layout.js",
                       "~/Content/assets/globals/scripts/index.js",

                      "~/Content/assets/globals/plugins/media/js/jquery.dataTables.min.js",
                      "~/Content/assets/globals/plugins/themes/bootstrap/dataTables.bootstrap.js",
                      "~/Content/assets/globals/scripts/tables-datatables.js",
                      "~/Content/assets/globals/plugins/chosen/chosen.jquery.min.js",

                      "~/Scripts/bootstrap.js",
                         "~/Scripts/jquery-ui.js"
                   //"~/Scripts/jquery.validate.js",
                   //"~/Scripts/jquery-validate.bootstrap-tooltip.js",
                   //"~/Scripts/tinymce/tinymce.min.js"

                   ));

            bundles.Add(new StyleBundle("~/bundles/cssForm").Include(

           "~/Content/assets/admin1/css/admin1.css",
           "~/Content/assets/globals/css/elements.css",
           "~/Content/assets/globals/css/plugins.css",
          "~/Content/assets/globals/plugins/media/css/jquery.dataTables.min.css",
          "~/Content/assets/globals/plugins/themes/bootstrap/dataTables.bootstrap.css"

               ));


            bundles.Add(new StyleBundle("~/bundles/css").Include(


                "~/Content/assets/admin1/css/admin1.css",
             "~/Content/assets/globals/css/elements.css",

             "~/Content/assets/globals/plugins/rickshaw/rickshaw.min.css",
             "~/Content/assets/globals/plugins/bxslider/jquery.bxslider.css",
             "~/Content/assets/globals/css/plugins.css",
            "~/Content/assets/globals/plugins/media/css/jquery.dataTables.min.css",
            "~/Content/assets/globals/plugins/themes/bootstrap/dataTables.bootstrap.css",
            "~/Content/jquery-ui.css"



                 ));
        }

    }
}
