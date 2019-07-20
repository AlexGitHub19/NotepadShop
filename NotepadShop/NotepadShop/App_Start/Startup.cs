using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using NotepadShop.BLL.Interfaces;
using Owin;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(NotepadShop.App_Start.Startup))]
namespace NotepadShop.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/"),
            });
        }

        private IUserService CreateUserService()
        {
            return DependencyResolver.Current.GetService<IUserService>();
        }
    }
}