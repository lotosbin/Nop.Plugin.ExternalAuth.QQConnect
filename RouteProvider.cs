// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.ExternalAuth.QQConnect.RouteProvider
// Assembly: Nop.Plugin.ExternalAuth.QQConnect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 40558BC5-EAEC-4D6B-BD95-AA1BA64F5702
// Assembly location: C:\Users\liubi\Downloads\21606_159662_ExternalAuth.QQConnect\Nop.Plugin.ExternalAuth.QQConnect.dll

using Nop.Web.Framework.Mvc.Routes;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nop.Plugin.ExternalAuth.QQConnect
{
  public class RouteProvider : IRouteProvider
  {
    public void RegisterRoutes(RouteCollection routes)
    {
      RouteCollectionExtensions.MapRoute(routes, "Plugin.ExternalAuth.QQConnect.Login", "Plugins/ExternalAuthQQConnect/Login", (object) new
      {
        controller = "ExternalAuthQQConnect",
        action = "Login"
      }, new string[1]
      {
        "Nop.Plugin.ExternalAuth.QQConnect.Controllers"
      });
      RouteCollectionExtensions.MapRoute(routes, "Plugin.ExternalAuth.QQConnect.LoginCallback", "Plugins/ExternalAuthQQConnect/LoginCallback", (object) new
      {
        controller = "ExternalAuthQQConnect",
        action = "LoginCallback"
      }, new string[1]
      {
        "Nop.Plugin.ExternalAuth.QQConnect.Controllers"
      });
    }

    public int Priority
    {
      get
      {
        return 0;
      }
    }
  }
}
