using Repos.Owin.Config;
using ReposCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Repos.Owin
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var config = ConfigurationManager.GetSection("ReposConfig") as IConfigurationSectionHandler;
            //initialize engine context
            EngineContext.Initialize(new OwinEngine(), config);
            
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
           // BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
