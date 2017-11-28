// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.ExternalAuth.QQConnect.Core.QQConnectClient
// Assembly: Nop.Plugin.ExternalAuth.QQConnect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 40558BC5-EAEC-4D6B-BD95-AA1BA64F5702
// Assembly location: C:\Users\liubi\Downloads\21606_159662_ExternalAuth.QQConnect\Nop.Plugin.ExternalAuth.QQConnect.dll

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
