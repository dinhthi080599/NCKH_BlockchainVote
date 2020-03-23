using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;
using System.Web.Routing;

namespace CSharpchainWebAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Election", "Election/{ID}/{action}",
                defaults: new { controller = "Election", action = "Index" }
                );
            routes.MapRoute(
                "CreateSignature", "CreateSignature/{ID}/{action}",
                defaults: new { controller = "CreateSignature", action = "Index" }
                );
            routes.MapRoute(
                "Voting", "Voting/{ID}/{action}",
                defaults: new { controller = "Voting", action = "Index" }
                );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
