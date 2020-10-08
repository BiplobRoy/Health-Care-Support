using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HealthCareSupport02
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start()
        {
            Session["Ad"] = new Models.Admin();
            Session["Do"] = new Models.Doctor();
            Session["Pa"] = new Models.Patient();
            Session["Adlogin"] = "";
            Session["Dologin"] = "";
            Session["Palogin"] = "";
            Session["msg"] = "";
            Session["Dng"] = "";
            Session["NT"] = "";
            Session["AP"] = "";
        }
    }
}
