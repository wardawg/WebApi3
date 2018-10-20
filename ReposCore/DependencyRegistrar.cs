using System.Configuration;
using Autofac;
using ProjectDependResolver;
using ReposCore.Factory.Rule;
using ReposCore.Factory.Edits;
using ReposCore.Factory.Edit;

namespace ReposCore
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, IConfigurationSectionHandler config)
        {
            //we cache presentation models between requests
            builder
                .RegisterType<DefaultDomainRule>()
                .As<IRule>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<DefaultServiceEntityEdit>()
                .As<IDomainEdit>()
                .InstancePerLifetimeScope();
            
        }



        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order
        {
            get { return 2; }
        }
    }
}
