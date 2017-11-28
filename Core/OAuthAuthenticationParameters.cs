// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.ExternalAuth.QQConnect.Core.OAuthAuthenticationParameters
// Assembly: Nop.Plugin.ExternalAuth.QQConnect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 40558BC5-EAEC-4D6B-BD95-AA1BA64F5702
// Assembly location: C:\Users\liubi\Downloads\21606_159662_ExternalAuth.QQConnect\Nop.Plugin.ExternalAuth.QQConnect.dll

using Nop.Services.Authentication.External;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.ExternalAuth.QQConnect.Core
{
  [Serializable]
  public class OAuthAuthenticationParameters : OpenAuthenticationParameters
  {
    private readonly string _providerSystemName;
    private IList<Nop.Services.Authentication.External.UserClaims> _claims;

    public OAuthAuthenticationParameters(string providerSystemName)
    {
      this.\u002Ector();
      this._providerSystemName = providerSystemName;
    }

    public virtual IList<Nop.Services.Authentication.External.UserClaims> UserClaims
    {
      get
      {
        return this._claims;
      }
    }

    public void AddClaim(Nop.Services.Authentication.External.UserClaims claim)
    {
      if (this._claims == null)
        this._claims = (IList<Nop.Services.Authentication.External.UserClaims>) new List<Nop.Services.Authentication.External.UserClaims>();
      ((ICollection<Nop.Services.Authentication.External.UserClaims>) this._claims).Add(claim);
    }

    public virtual string ProviderSystemName
    {
      get
      {
        return this._providerSystemName;
      }
    }
  }
}
