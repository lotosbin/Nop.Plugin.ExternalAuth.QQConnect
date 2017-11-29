using Nop.Core.Plugins;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Microsoft.AspNetCore.Routing;
using Nop.Core;

namespace Nop.Plugin.ExternalAuth.QQConnect
{
    public class QQConnectExternalAuthMethod : BasePlugin, IExternalAuthenticationMethod, IPlugin
    {
        private readonly ISettingService _settingService;
        private IWebHelper _webHelper;

        public QQConnectExternalAuthMethod(ISettingService settingService, IWebHelper webHelper)
        {
            this._settingService = settingService;
            _webHelper = webHelper;
        }


        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/QQConnectAuthentication/Configure";

        }

        public void GetPublicViewComponent(out string viewComponentName)
        {
            viewComponentName = "QQConnectAuthentication";
        }

        public virtual void Install()
        {
            this._settingService.SaveSetting(new QQConnectExternalAuthSettings()
            {
                ClientKeyIdentifier = "",
                ClientSecret = ""
            }, 0);
            LocalizationExtensions.AddOrUpdatePluginLocaleResource(this, "Plugins.ExternalAuth.QQConnect.Login", "Login using QQConnect account", null);
            LocalizationExtensions.AddOrUpdatePluginLocaleResource(this, "Plugins.ExternalAuth.QQConnect.CallbackUrl", "Callback Url", null);
            LocalizationExtensions.AddOrUpdatePluginLocaleResource(this, "Plugins.ExternalAuth.QQConnect.ClientKeyIdentifier", "App ID/API Key", null);
            LocalizationExtensions.AddOrUpdatePluginLocaleResource(this, "Plugins.ExternalAuth.QQConnect.ClientKeyIdentifier.Hint", "Enter your app ID/API key here. You can find it on your QQConnect application page.", null);
            LocalizationExtensions.AddOrUpdatePluginLocaleResource(this, "Plugins.ExternalAuth.QQConnect.ClientSecret", "App Secret", null);
            LocalizationExtensions.AddOrUpdatePluginLocaleResource(this, "Plugins.ExternalAuth.QQConnect.ClientSecret.Hint", "Enter your app secret here. You can find it on your QQConnect application page.", null);
            base.Install();
        }

        public virtual void Uninstall()
        {
            this._settingService.DeleteSetting<QQConnectExternalAuthSettings>();
            LocalizationExtensions.DeletePluginLocaleResource(this, "Plugins.ExternalAuth.QQConnect.Login");
            LocalizationExtensions.DeletePluginLocaleResource(this, "Plugins.ExternalAuth.QQConnect.CallbackUrl");
            LocalizationExtensions.DeletePluginLocaleResource(this, "Plugins.ExternalAuth.QQConnect.ClientKeyIdentifier");
            LocalizationExtensions.DeletePluginLocaleResource(this, "Plugins.ExternalAuth.QQConnect.ClientKeyIdentifier.Hint");
            LocalizationExtensions.DeletePluginLocaleResource(this, "Plugins.ExternalAuth.QQConnect.ClientSecret");
            LocalizationExtensions.DeletePluginLocaleResource(this, "Plugins.ExternalAuth.QQConnect.ClientSecret.Hint");
            base.Uninstall();
        }
    }
}
