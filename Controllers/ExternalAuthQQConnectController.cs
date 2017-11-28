// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.ExternalAuth.QQConnect.Controllers.ExternalAuthQQConnectController
// Assembly: Nop.Plugin.ExternalAuth.QQConnect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 40558BC5-EAEC-4D6B-BD95-AA1BA64F5702
// Assembly location: C:\Users\liubi\Downloads\21606_159662_ExternalAuth.QQConnect\Nop.Plugin.ExternalAuth.QQConnect.dll

using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Security;
using Nop.Core.Plugins;
using Nop.Plugin.ExternalAuth.QQConnect.Core;
using Nop.Plugin.ExternalAuth.QQConnect.Models;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Nop.Plugin.ExternalAuth.QQConnect.Controllers
{
  public class ExternalAuthQQConnectController : BasePluginController
  {
    private readonly ISettingService _settingService;
    private readonly IOAuthProviderQQConnectAuthorizer _oAuthProviderQQConnectAuthorizer;
    private readonly IOpenAuthenticationService _openAuthenticationService;
    private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
    private readonly IPermissionService _permissionService;
    private readonly IStoreContext _storeContext;
    private readonly IStoreService _storeService;
    private readonly IWorkContext _workContext;
    private readonly IPluginFinder _pluginFinder;
    private readonly ILocalizationService _localizationService;

    public ExternalAuthQQConnectController(ISettingService settingService, IOAuthProviderQQConnectAuthorizer oAuthProviderQQConnectAuthorizer, IOpenAuthenticationService openAuthenticationService, ExternalAuthenticationSettings externalAuthenticationSettings, IPermissionService permissionService, IStoreContext storeContext, IStoreService storeService, IWorkContext workContext, IPluginFinder pluginFinder, ILocalizationService localizationService)
    {
      this.\u002Ector();
      this._settingService = settingService;
      this._oAuthProviderQQConnectAuthorizer = oAuthProviderQQConnectAuthorizer;
      this._openAuthenticationService = openAuthenticationService;
      this._externalAuthenticationSettings = externalAuthenticationSettings;
      this._permissionService = permissionService;
      this._storeContext = storeContext;
      this._storeService = storeService;
      this._workContext = workContext;
      this._pluginFinder = pluginFinder;
      this._localizationService = localizationService;
    }

    [AdminAuthorize]
    [ChildActionOnly]
    public ActionResult Configure()
    {
      if (!this._permissionService.Authorize((PermissionRecord) StandardPermissionProvider.ManageExternalAuthenticationMethods))
        return (ActionResult) ((Controller) this).Content("Access denied");
      int scopeConfiguration = ((BaseController) this).GetActiveStoreScopeConfiguration(this._storeService, this._workContext);
      QQConnectExternalAuthSettings externalAuthSettings = (QQConnectExternalAuthSettings) this._settingService.LoadSetting<QQConnectExternalAuthSettings>(scopeConfiguration);
      ConfigurationModel configurationModel = new ConfigurationModel()
      {
        ClientKeyIdentifier = externalAuthSettings.ClientKeyIdentifier,
        ClientSecret = externalAuthSettings.ClientSecret,
        ActiveStoreScopeConfiguration = scopeConfiguration,
        CallbackUrl = this._oAuthProviderQQConnectAuthorizer.GenerateLocalCallbackUri().AbsolutePath
      };
      if (scopeConfiguration > 0)
      {
        configurationModel.ClientKeyIdentifier_OverrideForStore = (this._settingService.SettingExists<QQConnectExternalAuthSettings, string>((M0) externalAuthSettings, (Expression<Func<M0, M1>>) (x => x.ClientKeyIdentifier), scopeConfiguration) ? 1 : 0) != 0;
        configurationModel.ClientSecret_OverrideForStore = (this._settingService.SettingExists<QQConnectExternalAuthSettings, string>((M0) externalAuthSettings, (Expression<Func<M0, M1>>) (x => x.ClientSecret), scopeConfiguration) ? 1 : 0) != 0;
      }
      return (ActionResult) ((Controller) this).View("~/Plugins/ExternalAuth.QQConnect/Views/ExternalAuthQQConnect/Configure.cshtml", (object) configurationModel);
    }

    [HttpPost]
    [AdminAuthorize]
    [ChildActionOnly]
    public ActionResult Configure(ConfigurationModel model)
    {
      if (!this._permissionService.Authorize((PermissionRecord) StandardPermissionProvider.ManageExternalAuthenticationMethods))
        return (ActionResult) ((Controller) this).Content("Access denied");
      if (!((Controller) this).get_ModelState().get_IsValid())
        return this.Configure();
      int scopeConfiguration = ((BaseController) this).GetActiveStoreScopeConfiguration(this._storeService, this._workContext);
      QQConnectExternalAuthSettings externalAuthSettings = (QQConnectExternalAuthSettings) this._settingService.LoadSetting<QQConnectExternalAuthSettings>(scopeConfiguration);
      externalAuthSettings.ClientKeyIdentifier = model.ClientKeyIdentifier;
      externalAuthSettings.ClientSecret = model.ClientSecret;
      if (model.ClientKeyIdentifier_OverrideForStore || scopeConfiguration == 0)
        this._settingService.SaveSetting<QQConnectExternalAuthSettings, string>((M0) externalAuthSettings, (Expression<Func<M0, M1>>) (x => x.ClientKeyIdentifier), scopeConfiguration, 0 != 0);
      else if (scopeConfiguration > 0)
        this._settingService.DeleteSetting<QQConnectExternalAuthSettings, string>((M0) externalAuthSettings, (Expression<Func<M0, M1>>) (x => x.ClientKeyIdentifier), scopeConfiguration);
      if (model.ClientSecret_OverrideForStore || scopeConfiguration == 0)
        this._settingService.SaveSetting<QQConnectExternalAuthSettings, string>((M0) externalAuthSettings, (Expression<Func<M0, M1>>) (x => x.ClientSecret), scopeConfiguration, 0 != 0);
      else if (scopeConfiguration > 0)
        this._settingService.DeleteSetting<QQConnectExternalAuthSettings, string>((M0) externalAuthSettings, (Expression<Func<M0, M1>>) (x => x.ClientSecret), scopeConfiguration);
      this._settingService.ClearCache();
      ((BaseController) this).SuccessNotification(this._localizationService.GetResource("Admin.Plugins.Saved"), true);
      return this.Configure();
    }

    [ChildActionOnly]
    public ActionResult PublicInfo()
    {
      return (ActionResult) ((Controller) this).View("~/Plugins/ExternalAuth.QQConnect/Views/ExternalAuthQQConnect/PublicInfo.cshtml");
    }

    [NonAction]
    private ActionResult LoginInternal(string returnUrl, bool verifyResponse)
    {
      IExternalAuthenticationMethod authenticationMethod = this._openAuthenticationService.LoadExternalAuthenticationMethodBySystemName("ExternalAuth.QQConnect");
      if (authenticationMethod == null || !OpenAuthenticationExtensions.IsMethodActive(authenticationMethod, this._externalAuthenticationSettings) || !((IPlugin) authenticationMethod).get_PluginDescriptor().get_Installed() || !this._pluginFinder.AuthenticateStore(((IPlugin) authenticationMethod).get_PluginDescriptor(), ((BaseEntity) this._storeContext.get_CurrentStore()).get_Id()))
        throw new NopException("QQConnect module cannot be loaded");
      ((Controller) this).TryUpdateModel<LoginModel>((M0) new LoginModel());
      AuthorizeState authorizeState = ((IExternalProviderAuthorizer) this._oAuthProviderQQConnectAuthorizer).Authorize(returnUrl, new bool?(verifyResponse));
      switch (authorizeState.get_AuthenticationStatus() - 1)
      {
        case 0:
          if (!authorizeState.get_Success())
          {
            foreach (string error in (IEnumerable<string>) authorizeState.get_Errors())
              ExternalAuthorizerHelper.AddErrorsToDisplay(error);
          }
          return (ActionResult) new RedirectResult(UrlHelperExtensions.LogOn(((Controller) this).get_Url(), returnUrl));
        case 3:
          return (ActionResult) new RedirectResult(UrlHelperExtensions.LogOn(((Controller) this).get_Url(), returnUrl));
        case 4:
          return (ActionResult) ((Controller) this).RedirectToRoute("RegisterResult", (object) new
          {
            resultId = 2
          });
        case 5:
          return (ActionResult) ((Controller) this).RedirectToRoute("RegisterResult", (object) new
          {
            resultId = 3
          });
        case 6:
          return (ActionResult) ((Controller) this).RedirectToRoute("RegisterResult", (object) new
          {
            resultId = 1
          });
        default:
          if (authorizeState.get_Result() != null)
            return authorizeState.get_Result();
          return ((Controller) this).get_HttpContext().Request.IsAuthenticated ? (ActionResult) new RedirectResult(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/") : (ActionResult) new RedirectResult(UrlHelperExtensions.LogOn(((Controller) this).get_Url(), returnUrl));
      }
    }

    public ActionResult Login(string returnUrl)
    {
      return this.LoginInternal(returnUrl, false);
    }

    public ActionResult LoginCallback(string returnUrl)
    {
      return this.LoginInternal(returnUrl, true);
    }
  }
}
