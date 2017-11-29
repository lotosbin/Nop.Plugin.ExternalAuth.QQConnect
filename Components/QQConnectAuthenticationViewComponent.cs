using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.ExternalAuth.QQConnect.Components
{
    [ViewComponent(Name = "QQConnectAuthentication")]
    public class QQConnectAuthenticationViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Plugins/ExternalAuth.QQConnect/Views/PublicInfo.cshtml");
        }
    }
}