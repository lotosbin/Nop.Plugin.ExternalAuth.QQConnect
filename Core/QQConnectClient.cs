using BinbinDotNetOpenAuth.AspNet.Clients;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.ExternalAuth.QQConnect.Core
{
    public class QQConnectClient : QQClient
    {
        public QQConnectClient(string clientId, string clientSecret)
          : base(clientId, clientSecret)
        {
        }

        public QQConnectClient(string clientId, string clientSecret, params string[] requestedScopes)
          : base(clientId, clientSecret, requestedScopes)
        {
        }

        public new Uri GetServiceLoginUrl(Uri returnUrl)
        {
            return base.GetServiceLoginUrl(returnUrl);
        }

        public new IDictionary<string, string> GetUserData(string accessToken)
        {
            return base.GetUserData(accessToken);
        }

        public new string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            return base.QueryAccessToken(returnUrl, authorizationCode);
        }
    }
}
