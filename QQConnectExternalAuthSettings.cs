// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.ExternalAuth.QQConnect.QQConnectExternalAuthSettings
// Assembly: Nop.Plugin.ExternalAuth.QQConnect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 40558BC5-EAEC-4D6B-BD95-AA1BA64F5702
// Assembly location: C:\Users\liubi\Downloads\21606_159662_ExternalAuth.QQConnect\Nop.Plugin.ExternalAuth.QQConnect.dll

using Nop.Core.Configuration;

namespace Nop.Plugin.ExternalAuth.QQConnect
{
  public class QQConnectExternalAuthSettings : ISettings
  {
    public string ClientKeyIdentifier { get; set; }

    public string ClientSecret { get; set; }
  }
}
