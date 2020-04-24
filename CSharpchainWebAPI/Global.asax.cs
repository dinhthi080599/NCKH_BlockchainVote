using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CSharpchainWebAPI.Models;

namespace CSharpchainWebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static List<string>  node_list = new List<string>();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        public static void add_node(string new_node)
        {
            if (!node_list.Contains(new_node))
            {
                node_list.Add(new_node);
            }
        }
    }
}
