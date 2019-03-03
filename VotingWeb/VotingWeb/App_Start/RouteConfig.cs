using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VotingWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Hash",
                url: "vote{hash}",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "NoAction",
                url: "{controller}/{id}",
                defaults: new { controller = "Home", action = "Index", area = "", id = UrlParameter.Optional },
                constraints: new { id = @"\d+" },
                namespaces: new string[] { "VotingWeb.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", area = "", id = UrlParameter.Optional },
                namespaces: new string[] { "VotingWeb.Controllers" }
            );
        }
    }
}
