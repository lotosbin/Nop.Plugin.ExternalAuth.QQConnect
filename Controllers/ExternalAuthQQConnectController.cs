using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
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
            _settingService = settingService;
            _oAuthProviderQQConnectAuthorizer = oAuthProviderQQConnectAuthorizer;
            _openAuthenticationService = openAuthenticationService;
            _externalAuthenticationSettings = externalAuthenticationSettings;
            _permissionService = permissionService;
            _storeContext = storeContext;
            _storeService = storeService;
            _workContext = workContext;
            _pluginFinder = pluginFinder;
            _localizationService = localizationService;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return Content("Access denied");
            int scopeConfiguration = GetActiveStoreScopeConfiguration(_storeService, _workContext);
            QQConnectExternalAuthSettings externalAuthSettings = _settingService.LoadSetting<QQConnectExternalAuthSettings>(scopeConfiguration);
            ConfigurationModel configurationModel = new ConfigurationModel
            {
                ClientKeyIdentifier = externalAuthSettings.ClientKeyIdentifier,
                ClientSecret = externalAuthSettings.ClientSecret,
                ActiveStoreScopeConfiguration = scopeConfiguration,
                CallbackUrl = _oAuthProviderQQConnectAuthorizer.GenerateLocalCallbackUri().AbsolutePath
            };
            if (scopeConfiguration > 0)
            {
                configurationModel.ClientKeyIdentifier_OverrideForStore = (_settingService.SettingExists(externalAuthSettings,
        x => x.ClientKeyIdentifier, scopeConfiguration) ? 1 : 0) != 0;
                configurationModel.ClientSecret_OverrideForStore = (_settingService.SettingExists(externalAuthSettings, x => x.ClientSecret, scopeConfiguration) ? 1 : 0) != 0;
            }
            return View("~/Plugins/ExternalAuth.QQConnect/Views/ExternalAuthQQConnect/Configure.cshtml", configurationModel);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return Content("Access denied");
            if (!ModelState.IsValid)
                return Configure();
            int scopeConfiguration = GetActiveStoreScopeConfiguration(_storeService, _workContext);
            QQConnectExternalAuthSettings externalAuthSettings = _settingService.LoadSetting<QQConnectExternalAuthSettings>(scopeConfiguration);
            externalAuthSettings.ClientKeyIdentifier = model.ClientKeyIdentifier;
            externalAuthSettings.ClientSecret = model.ClientSecret;
            if (model.ClientKeyIdentifier_OverrideForStore || scopeConfiguration == 0)
                _settingService.SaveSetting(externalAuthSettings, x => x.ClientKeyIdentifier, scopeConfiguration, false);
            else if (scopeConfiguration > 0)
                _settingService.DeleteSetting(externalAuthSettings, x => x.ClientKeyIdentifier, scopeConfiguration);
            if (model.ClientSecret_OverrideForStore || scopeConfiguration == 0)
                _settingService.SaveSetting(externalAuthSettings, x => x.ClientSecret, scopeConfiguration, false);
            else if (scopeConfiguration > 0)
                _settingService.DeleteSetting(externalAuthSettings, x => x.ClientSecret, scopeConfiguration);
            _settingService.ClearCache();
            this.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"), true);
            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo()
        {
            return this.View("~/Plugins/ExternalAuth.QQConnect/Views/ExternalAuthQQConnect/PublicInfo.cshtml");
        }

        [NonAction]
        private ActionResult LoginInternal(string returnUrl, bool verifyResponse)
        {
            IExternalAuthenticationMethod authenticationMethod = _openAuthenticationService.LoadExternalAuthenticationMethodBySystemName("ExternalAuth.QQConnect");
            if (authenticationMethod == null || !authenticationMethod.IsMethodActive(_externalAuthenticationSettings) || !authenticationMethod.PluginDescriptor.Installed || !_pluginFinder.AuthenticateStore(authenticationMethod.PluginDescriptor, _storeContext.CurrentStore.Id))
                throw new NopException("QQConnect module cannot be loaded");
            TryUpdateModel(new LoginModel());
            AuthorizeState authorizeState = _oAuthProviderQQConnectAuthorizer.Authorize(returnUrl, verifyResponse);
            switch ((int)authorizeState.AuthenticationStatus - 1)
            {
                case 0:
                    if (!authorizeState.Success)
                    {
                        foreach (string error in authorizeState.Errors)
                        {
                            ExternalAuthorizerHelper.AddErrorsToDisplay(error);
                        }
                    }
                    return new RedirectResult(UrlHelperExtensions.LogOn(this.Url, returnUrl));
                case 3:
                    return new RedirectResult(UrlHelperExtensions.LogOn(this.Url, returnUrl));
                case 4:
                    return this.RedirectToRoute("RegisterResult", new
                    {
                        resultId = 2
                    });
                case 5:
                    return this.RedirectToRoute("RegisterResult", new
                    {
                        resultId = 3
                    });
                case 6:
                    return this.RedirectToRoute("RegisterResult", new
                    {
                        resultId = 1
                    });
                default:
                    if (authorizeState.Result != null)
                        return authorizeState.Result;
                    return this.HttpContext.Request.IsAuthenticated ? new RedirectResult(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/") : new RedirectResult(UrlHelperExtensions.LogOn(this.Url, returnUrl));
            }
        }

        public ActionResult Login(string returnUrl)
        {
            return LoginInternal(returnUrl, false);
        }

        public ActionResult LoginCallback(string returnUrl)
        {
            return LoginInternal(returnUrl, true);
        }
    }
}
