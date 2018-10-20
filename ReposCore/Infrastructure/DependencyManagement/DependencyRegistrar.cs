using ProjectDependResolver;
using System.Configuration;
using Autofac;
using System;
using DependencyResolver;
using System.Web;

namespace ReposCore.Infrastructure.DependencyManagement
{
    public class DependencyRegistrar : BaseDependencyRegistrar
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
         , IConfigOptions options )
        {
            //we cache presentation models between requests

         
        }

    

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public override int Order
        {
            get { return 2; }
        }

        public override RegType RegisterType { get { return RegType.common; } }

        
    }
}
