using Nop.Core.Configuration;

namespace Nop.Plugin.ExternalAuth.QQConnect
{
    public class QQConnectExternalAuthSettings : ISettings
    {
        public string ClientKeyIdentifier { get; set; }

        public string ClientSecret { get; set; }
    }
}
