using System.Configuration;
using Autofac;
using ProjectDependResolver;
using ReposData.Repository;
using ReposCore.Configuration;
using ReposCore.Caching;
using DependencyResolver;

namespace ReposData
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
         , IConfigOptions options)
        {
            //we cache presentation models between requests

            builder
                .RegisterGeneric(typeof(EfRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();

            var lconfig = (ReposConfig)config;

                        
            if (!Container.IsRegistered<IDbContext>())
                builder
                 .Register<IDbContext>(c => new ReposContext(lconfig.ContextName))
                 .InstancePerLifetimeScope();

            builder
                .RegisterType<PerRequestCacheManager>()
                .As<ICacheManager>()
                .Named<ICacheManager>("repos_cache_per_request")
                .InstancePerLifetimeScope();
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
