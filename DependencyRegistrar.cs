using Autofac;
using Autofac.Builder;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.ExternalAuth.QQConnect.Core;
using RegistrationExtensions = Autofac.RegistrationExtensions;

namespace Nop.Plugin.ExternalAuth.QQConnect
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<QQConnectProviderAuthorizer>().As<IOAuthProviderQQConnectAuthorizer>().InstancePerLifetimeScope();
        }

        public int Order => 1;
    }
}
