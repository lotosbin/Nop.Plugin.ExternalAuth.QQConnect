// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.ExternalAuth.QQConnect.DependencyRegistrar
// Assembly: Nop.Plugin.ExternalAuth.QQConnect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 40558BC5-EAEC-4D6B-BD95-AA1BA64F5702
// Assembly location: C:\Users\liubi\Downloads\21606_159662_ExternalAuth.QQConnect\Nop.Plugin.ExternalAuth.QQConnect.dll

using Autofac;
using Autofac.Builder;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.ExternalAuth.QQConnect.Core;

namespace Nop.Plugin.ExternalAuth.QQConnect
{
  public class DependencyRegistrar : IDependencyRegistrar
  {
    public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
    {
      ((IRegistrationBuilder<QQConnectProviderAuthorizer, ConcreteReflectionActivatorData, SingleRegistrationStyle>) RegistrationExtensions.RegisterType<QQConnectProviderAuthorizer>(builder)).As<IOAuthProviderQQConnectAuthorizer>().InstancePerLifetimeScope();
    }

    public int Order
    {
      get
      {
        return 1;
      }
    }
  }
}
