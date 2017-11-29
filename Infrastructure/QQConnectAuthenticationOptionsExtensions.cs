using Microsoft.AspNetCore.Authentication;
using System;
using Microsoft.AspNetCore.Authentication.QQConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class QQConnectAuthenticationOptionsExtensions
    {
        public static AuthenticationBuilder AddQQConnect(this AuthenticationBuilder builder)
        {
            return builder.AddQQConnect("QQConnect", _ => { });
        }

        public static AuthenticationBuilder AddQQConnect(this AuthenticationBuilder builder, Action<QQConnectOptions> configureOptions)
        {
            return builder.AddQQConnect("QQConnect", configureOptions);
        }

        public static AuthenticationBuilder AddQQConnect(this AuthenticationBuilder builder, string authenticationScheme, Action<QQConnectOptions> configureOptions)
        {
            return builder.AddQQConnect(authenticationScheme, QQConnectDefaults.DisplayName, configureOptions);
        }

        public static AuthenticationBuilder AddQQConnect(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<QQConnectOptions> configureOptions)
        {
            return builder.AddOAuth<QQConnectOptions, QQConnectHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}