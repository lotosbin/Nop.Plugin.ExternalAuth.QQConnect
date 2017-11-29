

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.QQConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Plugins;
using Nop.Plugin.ExternalAuth.QQConnect.Models;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.ExternalAuth.QQConnect.Controllers
{
    public class QQConnectAuthenticationController : BasePluginController
    {
        private readonly ISettingService _settingService;
        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly IExternalAuthenticationService _externalAuthenticationService;
        private readonly QQConnectExternalAuthSettings _facebookExternalAuthSettings;
        private readonly IOptionsMonitorCache<QQConnectOptions> _optionsCache;

        public QQConnectAuthenticationController(ISettingService settingService, ExternalAuthenticationSettings externalAuthenticationSettings, IPermissionService permissionService, IStoreContext storeContext, IStoreService storeService, ILocalizationService localizationService, IExternalAuthenticationService externalAuthenticationService, QQConnectExternalAuthSettings facebookExternalAuthSettings, IOptionsMonitorCache<QQConnectOptions> optionsCache)
        {
            _settingService = settingService;
            _permissionService = permissionService;
            _localizationService = localizationService;
            _externalAuthenticationService = externalAuthenticationService;
            _facebookExternalAuthSettings = facebookExternalAuthSettings;
            _optionsCache = optionsCache;
        }

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            var model = new ConfigurationModel
            {
                CallbackUrl = new QQConnectOptions().CallbackPath,
                ClientKeyIdentifier = _facebookExternalAuthSettings.ClientKeyIdentifier,
                ClientSecret = _facebookExternalAuthSettings.ClientSecret
            };

            return View("~/Plugins/ExternalAuth.QQConnect/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAntiForgery]
        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Configure();

            //save settings
            _facebookExternalAuthSettings.ClientKeyIdentifier = model.ClientKeyIdentifier;
            _facebookExternalAuthSettings.ClientSecret = model.ClientSecret;
            _settingService.SaveSetting(_facebookExternalAuthSettings);

            //clear Facebook authentication options cache
            _optionsCache.TryRemove(QQConnectDefaults.AuthenticationScheme);

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }


        public IActionResult Login(string returnUrl)
        {
            if (!_externalAuthenticationService.ExternalAuthenticationMethodIsAvailable(QQConnectAuthenticationDefaults.ProviderSystemName))
                throw new NopException("QQConnect authentication module cannot be loaded");

            if (string.IsNullOrEmpty(_facebookExternalAuthSettings.ClientKeyIdentifier) || string.IsNullOrEmpty(_facebookExternalAuthSettings.ClientSecret))
                throw new NopException("QQConnect authentication module not configured");

            //configure login callback action
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("LoginCallback", "QQConnectAuthentication", new { returnUrl = returnUrl })
            };

            return Challenge(authenticationProperties, QQConnectDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> LoginCallback(string returnUrl)
        {
            //authenticate Facebook user
            var authenticateResult = await this.HttpContext.AuthenticateAsync(QQConnectDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded || !authenticateResult.Principal.Claims.Any())
                return RedirectToRoute("Login");

            //create external authentication parameters
            var authenticationParameters = new ExternalAuthenticationParameters
            {
                ProviderSystemName = QQConnectAuthenticationDefaults.ProviderSystemName,
                AccessToken = await this.HttpContext.GetTokenAsync(QQConnectDefaults.AuthenticationScheme, "access_token"),
                Email = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email)?.Value,
                ExternalIdentifier = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value,
                ExternalDisplayIdentifier = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Name)?.Value,
                Claims = authenticateResult.Principal.Claims.Select(claim => new ExternalAuthenticationClaim(claim.Type, claim.Value)).ToList()
            };

            //authenticate Nop user
            return _externalAuthenticationService.Authenticate(authenticationParameters, returnUrl);
        }

    }
}
