using Ninject;
using Ninject.Web.Mvc;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NotepadShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var kernel = new StandardKernel(new NinjectSettings() { LoadExtensions = false });
            kernel.Load("*.dll");
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}
