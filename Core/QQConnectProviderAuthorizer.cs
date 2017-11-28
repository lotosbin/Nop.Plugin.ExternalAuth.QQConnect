// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.ExternalAuth.QQConnect.Core.QQConnectProviderAuthorizer
// Assembly: Nop.Plugin.ExternalAuth.QQConnect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 40558BC5-EAEC-4D6B-BD95-AA1BA64F5702
// Assembly location: C:\Users\liubi\Downloads\21606_159662_ExternalAuth.QQConnect\Nop.Plugin.ExternalAuth.QQConnect.dll

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

    private QQConnectClient QQConnectApplication
    {
      get
      {
        return this._qqConnectApplication ?? (this._qqConnectApplication = new QQConnectClient(this._qqConnectExternalAuthSettings.ClientKeyIdentifier, this._qqConnectExternalAuthSettings.ClientSecret));
      }
    }

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
        authenticationParameters.set_ExternalIdentifier(providerUserId1);
        string str = authenticationResult.ExtraData["accesstoken"];
        authenticationParameters.set_OAuthToken(str);
        string providerUserId2 = authenticationResult.ProviderUserId;
        authenticationParameters.set_OAuthAccessToken(providerUserId2);
        OAuthAuthenticationParameters parameters = authenticationParameters;
        if (this._externalAuthenticationSettings.get_AutoRegisterEnabled())
          this.ParseClaims(authenticationResult, parameters);
        AuthorizationResult authorizationResult = this._authorizer.Authorize((OpenAuthenticationParameters) parameters);
        return new AuthorizeState(returnUrl, authorizationResult);
      }
      AuthorizeState authorizeState = new AuthorizeState(returnUrl, (OpenAuthenticationStatus) 1);
      Exception error = authenticationResult.Error;
      string str1 = (error != null ? error.Message : (string) null) ?? "Unknown error";
      authorizeState.AddError(str1);
      return authorizeState;
    }

    private void ParseClaims(AuthenticationResult authenticationResult, OAuthAuthenticationParameters parameters)
    {
      UserClaims userClaims = new UserClaims();
      NameClaims nameClaims = new NameClaims();
      userClaims.set_Name(nameClaims);
      ContactClaims contactClaims = new ContactClaims();
      string str1 = string.Format("{0}@qq.com", (object) authenticationResult.ProviderUserId);
      contactClaims.set_Email(str1);
      userClaims.set_Contact(contactClaims);
      UserClaims claim = userClaims;
      if (authenticationResult.ExtraData.ContainsKey("name"))
      {
        string str2 = authenticationResult.ExtraData["name"];
        if (!string.IsNullOrEmpty(str2))
          claim.get_Name().set_Nickname(str2);
      }
      parameters.AddClaim(claim);
    }

    private AuthorizeState RequestAuthentication()
    {
      string absoluteUri = this.QQConnectApplication.GetServiceLoginUrl(this.GenerateLocalCallbackUri()).AbsoluteUri;
      AuthorizeState authorizeState = new AuthorizeState("", (OpenAuthenticationStatus) 3);
      RedirectResult redirectResult = new RedirectResult(absoluteUri);
      authorizeState.set_Result((ActionResult) redirectResult);
      return authorizeState;
    }

    public Uri GenerateLocalCallbackUri()
    {
      return new Uri(string.Format("{0}plugins/externalauthQQConnect/logincallback/", (object) this._webHelper.GetStoreLocation()));
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
