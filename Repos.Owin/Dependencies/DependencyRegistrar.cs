using Autofac;
using DependencyResolver;
using ProjectDependResolver;
using ReposCore.Caching;
using System.Configuration;

namespace Repos.Owin.Dependecies
{
    public class DependencyRegistrar : BaseDependencyRegistrar
    {
        public override void Register(ContainerBuilder builder
                                    , IContainer Container
                                    , ITypeFinder typeFinder
                                    , IConfigurationSectionHandler config
                                    , IConfigOptions options)
        {
           
            builder
                .RegisterType<PerRequestCacheManager>()
                .As<ICacheManager>()
                .Named<ICacheManager>("repos_cache_per_request")
                .InstancePerLifetimeScope();

            builder
                .RegisterType<MemoryCacheManager>()
                .As<ICacheManager>()
                .Named<ICacheManager>("repos_cache_static")
                .SingleInstance();

        }

    }
}