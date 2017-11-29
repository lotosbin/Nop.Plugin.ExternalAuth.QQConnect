using Microsoft.AspNetCore.Authentication.QQConnect;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;

namespace Nop.Plugin.ExternalAuth.QQConnect.Models
{
    public class ConfigurationModel : BaseNopModel
    {

        [NopResourceDisplayName("Plugins.ExternalAuth.QQConnect.ClientKeyIdentifier")]
        public string ClientKeyIdentifier { get; set; }


        [NopResourceDisplayName("Plugins.ExternalAuth.QQConnect.ClientSecret")]
        public string ClientSecret { get; set; }


        [NopResourceDisplayName("Plugins.ExternalAuth.QQConnect.CallbackUrl")]
        public string CallbackUrl { get; set; }
    }
}
