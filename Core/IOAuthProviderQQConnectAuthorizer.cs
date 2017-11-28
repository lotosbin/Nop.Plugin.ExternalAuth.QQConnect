// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.ExternalAuth.QQConnect.Core.IOAuthProviderQQConnectAuthorizer
// Assembly: Nop.Plugin.ExternalAuth.QQConnect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 40558BC5-EAEC-4D6B-BD95-AA1BA64F5702
// Assembly location: C:\Users\liubi\Downloads\21606_159662_ExternalAuth.QQConnect\Nop.Plugin.ExternalAuth.QQConnect.dll

using Nop.Services.Authentication.External;
using System;

namespace Nop.Plugin.ExternalAuth.QQConnect.Core
{
  public interface IOAuthProviderQQConnectAuthorizer : IExternalProviderAuthorizer
  {
    Uri GenerateLocalCallbackUri();
  }
}
