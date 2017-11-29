using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.QQConnect;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Services.Authentication.External;

namespace Nop.Plugin.ExternalAuth.QQConnect.Infrastructure
{
    /// <summary>
    /// Registration of QQConnect authentication service (plugin)
    /// </summary>
    public class QQConnectAuthenticationRegistrar : IExternalAuthenticationRegistrar
    {
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="builder">Authentication builder</param>
        public void Configure(AuthenticationBuilder builder)
        {

            builder.AddQQConnect(QQConnectDefaults.AuthenticationScheme, options =>
            {
                var settings = EngineContext.Current.Resolve<QQConnectExternalAuthSettings>();

                options.AppId = settings.ClientKeyIdentifier;
                options.AppKey = settings.ClientSecret;
                options.SaveTokens = true;
            });
        }
    }
}
