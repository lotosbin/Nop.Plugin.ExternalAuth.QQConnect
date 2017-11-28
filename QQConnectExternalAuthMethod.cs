// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.ExternalAuth.QQConnect.QQConnectExternalAuthMethod
// Assembly: Nop.Plugin.ExternalAuth.QQConnect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 40558BC5-EAEC-4D6B-BD95-AA1BA64F5702
// Assembly location: C:\Users\liubi\Downloads\21606_159662_ExternalAuth.QQConnect\Nop.Plugin.ExternalAuth.QQConnect.dll

using Nop.Core.Plugins;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using System.Web.Routing;

namespace Nop.Plugin.ExternalAuth.QQConnect
{
  public class QQConnectExternalAuthMethod : BasePlugin, IExternalAuthenticationMethod, IPlugin
  {
    private readonly ISettingService _settingService;

    public QQConnectExternalAuthMethod(ISettingService settingService)
    {
      this.\u002Ector();
      this._settingService = settingService;
    }

    public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
    {
      actionName = "Configure";
      controllerName = "ExternalAuthQQConnect";
      routeValues = new RouteValueDictionary()
      {
        {
          "Namespaces",
          (object) "Nop.Plugin.ExternalAuth.QQConnect.Controllers"
        },
        {
          "area",
          (object) null
        }
      };
    }

    public void GetPublicInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
    {
      actionName = "PublicInfo";
      controllerName = "ExternalAuthQQConnect";
      routeValues = new RouteValueDictionary()
      {
        {
          "Namespaces",
          (object) "Nop.Plugin.ExternalAuth.QQConnect.Controllers"
        },
        {
          "area",
          (object) null
        }
      };
    }

    public virtual void Install()
    {
      this._settingService.SaveSetting<QQConnectExternalAuthSettings>((M0) new QQConnectExternalAuthSettings()
      {
        ClientKeyIdentifier = "",
        ClientSecret = ""
      }, 0);
      LocalizationExtensions.AddOrUpdatePluginLocaleResource((BasePlugin) this, "Plugins.ExternalAuth.QQConnect.Login", "Login using QQConnect account", (string) null);
      LocalizationExtensions.AddOrUpdatePluginLocaleResource((BasePlugin) this, "Plugins.ExternalAuth.QQConnect.CallbackUrl", "Callback Url", (string) null);
      LocalizationExtensions.AddOrUpdatePluginLocaleResource((BasePlugin) this, "Plugins.ExternalAuth.QQConnect.ClientKeyIdentifier", "App ID/API Key", (string) null);
      LocalizationExtensions.AddOrUpdatePluginLocaleResource((BasePlugin) this, "Plugins.ExternalAuth.QQConnect.ClientKeyIdentifier.Hint", "Enter your app ID/API key here. You can find it on your QQConnect application page.", (string) null);
      LocalizationExtensions.AddOrUpdatePluginLocaleResource((BasePlugin) this, "Plugins.ExternalAuth.QQConnect.ClientSecret", "App Secret", (string) null);
      LocalizationExtensions.AddOrUpdatePluginLocaleResource((BasePlugin) this, "Plugins.ExternalAuth.QQConnect.ClientSecret.Hint", "Enter your app secret here. You can find it on your QQConnect application page.", (string) null);
      base.Install();
    }

    public virtual void Uninstall()
    {
      this._settingService.DeleteSetting<QQConnectExternalAuthSettings>();
      LocalizationExtensions.DeletePluginLocaleResource((BasePlugin) this, "Plugins.ExternalAuth.QQConnect.Login");
      LocalizationExtensions.DeletePluginLocaleResource((BasePlugin) this, "Plugins.ExternalAuth.QQConnect.CallbackUrl");
      LocalizationExtensions.DeletePluginLocaleResource((BasePlugin) this, "Plugins.ExternalAuth.QQConnect.ClientKeyIdentifier");
      LocalizationExtensions.DeletePluginLocaleResource((BasePlugin) this, "Plugins.ExternalAuth.QQConnect.ClientKeyIdentifier.Hint");
      LocalizationExtensions.DeletePluginLocaleResource((BasePlugin) this, "Plugins.ExternalAuth.QQConnect.ClientSecret");
      LocalizationExtensions.DeletePluginLocaleResource((BasePlugin) this, "Plugins.ExternalAuth.QQConnect.ClientSecret.Hint");
      base.Uninstall();
    }
  }
}
