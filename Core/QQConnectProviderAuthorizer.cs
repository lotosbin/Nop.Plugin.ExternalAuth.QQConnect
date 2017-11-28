
using DotNetOpenAuth.AspNet;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Authentication.External;
using System;
using System.Web;
using System.Web.Mvc;

namespace Nop.Plugin.ExternalAuth.QQConnect.Core
{
    public class QQConnectProviderAuthorizer : IOAuthProviderQQConnectAuthorizer, IExternalProviderAuthorizer
    {
        private readonly IExternalAuthorizer _authorizer;
        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
        private readonly QQConnectExternalAuthSettings _qqConnectExternalAuthSettings;
        private readonly HttpContextBase _httpContext;
        private readonly IWebHelper _webHelper;
        private QQConnectClient _qqConnectApplication;

        public QQConnectProviderAuthorizer(IExternalAuthorizer authorizer, ExternalAuthenticationSettings externalAuthenticationSettings, QQConnectExternalAuthSettings qqConnectExternalAuthSettings, HttpContextBase httpContext, IWebHelper webHelper)
        {
            this._authorizer = authorizer;
            this._externalAuthenticationSettings = externalAuthenticationSettings;
            this._qqConnectExternalAuthSettings = qqConnectExternalAuthSettings;
            this._httpContext = httpContext;
            this._webHelper = webHelper;
        }

        private QQConnectClient QQConnectApplication => this._qqConnectApplication ?? (this._qqConnectApplication = new QQConnectClient(this._qqConnectExternalAuthSettings.ClientKeyIdentifier, this._qqConnectExternalAuthSettings.ClientSecret));

        private AuthorizeState VerifyAuthentication(string returnUrl)
        {
            AuthenticationResult authenticationResult = this.QQConnectApplication.VerifyAuthentication(this._httpContext, this.GenerateLocalCallbackUri());
            if (authenticationResult.IsSuccessful)
            {
                if (!authenticationResult.ExtraData.ContainsKey("id"))
                    throw new Exception("Authentication result does not contain id data");
                if (!authenticationResult.ExtraData.ContainsKey("accesstoken"))
                    throw new Exception("Authentication result does not contain accesstoken data");
                OAuthAuthenticationParameters authenticationParameters = new OAuthAuthenticationParameters(Provider.SystemName);
                string providerUserId1 = authenticationResult.ProviderUserId;
                authenticationParameters.ExternalIdentifier = providerUserId1;
                string str = authenticationResult.ExtraData["accesstoken"];
                authenticationParameters.OAuthToken = str;
                string providerUserId2 = authenticationResult.ProviderUserId;
                authenticationParameters.OAuthAccessToken = providerUserId2;
                OAuthAuthenticationParameters parameters = authenticationParameters;
                if (this._externalAuthenticationSettings.AutoRegisterEnabled)
                    this.ParseClaims(authenticationResult, parameters);
                AuthorizationResult authorizationResult = this._authorizer.Authorize(parameters);
                return new AuthorizeState(returnUrl, authorizationResult);
            }
            AuthorizeState authorizeState = new AuthorizeState(returnUrl, (OpenAuthenticationStatus)1);
            Exception error = authenticationResult.Error;
            string str1 = error?.Message ?? "Unknown error";
            authorizeState.AddError(str1);
            return authorizeState;
        }

        private void ParseClaims(AuthenticationResult authenticationResult, OAuthAuthenticationParameters parameters)
        {
            UserClaims userClaims = new UserClaims
            {
                Name = new NameClaims(),
                Contact = new ContactClaims
                {
                    Email = $"{authenticationResult.ProviderUserId}@qq.com"
                }
            };
            if (authenticationResult.ExtraData.ContainsKey("name"))
            {
                string name = authenticationResult.ExtraData["name"];
                if (!string.IsNullOrEmpty(name))
                    userClaims.Name.Nickname = name;
            }
            parameters.AddClaim(userClaims);
        }

        private AuthorizeState RequestAuthentication()
        {
            string absoluteUri = this.QQConnectApplication.GetServiceLoginUrl(this.GenerateLocalCallbackUri()).AbsoluteUri;
            AuthorizeState authorizeState = new AuthorizeState("", (OpenAuthenticationStatus)3);
            RedirectResult redirectResult = new RedirectResult(absoluteUri);
            authorizeState.Result = redirectResult;
            return authorizeState;
        }

        public Uri GenerateLocalCallbackUri()
        {
            return new Uri($"{this._webHelper.GetStoreLocation()}plugins/externalauthQQConnect/logincallback/");
        }

        public AuthorizeState Authorize(string returnUrl, bool? verifyResponse = null)
        {
            if (!verifyResponse.HasValue)
                throw new ArgumentException("QQConnect plugin cannot automatically determine verifyResponse property");
            if (verifyResponse.Value)
                return this.VerifyAuthentication(returnUrl);
            return this.RequestAuthentication();
        }
    }
}
