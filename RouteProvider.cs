using Nop.Web.Framework.Mvc.Routes;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nop.Plugin.ExternalAuth.QQConnect
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.ExternalAuth.QQConnect.Login", "Plugins/ExternalAuthQQConnect/Login", new
            {
                controller = "ExternalAuthQQConnect",
                action = "Login"
            }, new string[1]
            {
                "Nop.Plugin.ExternalAuth.QQConnect.Controllers"
            });
            routes.MapRoute("Plugin.ExternalAuth.QQConnect.LoginCallback", "Plugins/ExternalAuthQQConnect/LoginCallback", new
            {
                controller = "ExternalAuthQQConnect",
                action = "LoginCallback"
            }, new string[1]
            {
                "Nop.Plugin.ExternalAuth.QQConnect.Controllers"
            });
        }

        public int Priority => 0;
    }
}
