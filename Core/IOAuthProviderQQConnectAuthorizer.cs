using Nop.Services.Authentication.External;
using System;

namespace Nop.Plugin.ExternalAuth.QQConnect.Core
{
    public interface IOAuthProviderQQConnectAuthorizer : IExternalProviderAuthorizer
    {
        Uri GenerateLocalCallbackUri();
    }
}
