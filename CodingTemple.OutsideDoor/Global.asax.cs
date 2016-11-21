﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace CodingTemple.OutsideDoor
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
