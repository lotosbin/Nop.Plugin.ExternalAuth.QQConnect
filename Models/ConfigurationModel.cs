using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.ExternalAuth.QQConnect.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.ExternalAuth.QQConnect.ClientKeyIdentifier")]
        public string ClientKeyIdentifier { get; set; }

        public bool ClientKeyIdentifier_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.ExternalAuth.QQConnect.ClientSecret")]
        public string ClientSecret { get; set; }

        public bool ClientSecret_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.ExternalAuth.QQConnect.CallbackUrl")]
        public string CallbackUrl { get; set; }
    }
}
