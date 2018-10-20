using Autofac;
using DependencyResolver;
using ProjectDependResolver;
using Repos.DomainModel.Interface.Atrributes.DynamicAttributes;
using Repos.DomainModel.Interface.Interfaces.DomainList;
using System.Configuration;
using System.Collections.Generic;

namespace Repos.DomainModel.Interface
{
    public class DependencyRegistrar    
        : BaseDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public override void Register(ContainerBuilder builder
           , IContainer Container
           , ITypeFinder typeFinder
           , IConfigurationSectionHandler config
           , IConfigOptions options)
        {
            
            if (!Container.IsRegistered<IListFactory>())
                builder
                .RegisterType<DefaultListFactory>()
                .As<IListFactory>()
                .InstancePerLifetimeScope();
           
           

            if (!Container.IsRegistered<FilterKeyPair>())
                builder
                .RegisterType<FilterKeyPair>()
                .As<IFilterKeyPair>()
                .InstancePerLifetimeScope();
            
        }
               

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public override int Order
        {
            get { return 0; }
        }

        public override RegType RegisterType { get { return RegType.set_Interface_defaults; } }
       
    }
}
