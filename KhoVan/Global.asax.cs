using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MvcApplication5
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            System.Net.ServicePointManager.CertificatePolicy = new Controllers.CustomCertificatePolicy();
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
        void Application_End(object sender, EventArgs e)
        {
            // Code that runs on application shutdown
        }
        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
        }
        public void Session_Start(object sender, EventArgs e)
        {
            //Session["UserId"] = User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1);
            //Session["PrsnId"] = (new BasePage()).GetDescription("PRSN", Session["UserId"].ToString());
            //if (string.IsNullOrEmpty(Session["PrsnId"].ToString()))
            //{ 
            // Response.Redirect(Request.ApplicationPath + \\AppStructure\\Error.aspx?InvalidUser=yes);
            //} 
            // Response.Redirect("SessionExpired.aspx");
        }
        void Session_End(object sender, EventArgs e)
        {

            // Is this where the problem is? I want to kill all session variables at the timeout. 
            try
            {
                HttpContext.Current.Session.Abandon();
            }
            catch (Exception ex)
            {


            }

            //Response.Redirect("SessionExpired.aspx");

        }

    }
}