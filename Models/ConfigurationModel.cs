// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.ExternalAuth.QQConnect.Models.ConfigurationModel
// Assembly: Nop.Plugin.ExternalAuth.QQConnect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 40558BC5-EAEC-4D6B-BD95-AA1BA64F5702
// Assembly location: C:\Users\liubi\Downloads\21606_159662_ExternalAuth.QQConnect\Nop.Plugin.ExternalAuth.QQConnect.dll

using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.ExternalAuth.QQConnect.Models
{
  public class ConfigurationModel : BaseNopModel
  {
    public ConfigurationModel()
    {
      base.\u002Ector();
    }

    public int ActiveStoreScopeConfiguration { get; set; }

    [NopResourceDisplayName("Plugins.ExternalAuth.QQConnect.ClientKeyIdentifier")]
    public string ClientKeyIdentifier { get; set; }

    public bool ClientKeyIdentifier_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.ExternalAuth.QQConnect.ClientSecret")]
    public string ClientSecret { get; set; }

    public bool ClientSecret_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.ExternalAuth.QQConnect.CallbackUrl")]
    public string CallbackUrl { get; set; }
  }
}
