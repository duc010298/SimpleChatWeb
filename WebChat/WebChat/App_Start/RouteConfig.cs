﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebChat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "WebChat", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Authentication",
                "Authentication/{action}/{id}",
                defaults: new { controller = "Authentication", action = "Form", id = UrlParameter.Optional }
            );
        }
    }
}
