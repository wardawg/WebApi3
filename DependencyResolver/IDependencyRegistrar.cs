using Autofac;
using DependencyResolver;
using System.Collections.Generic;
using System.Configuration;

namespace ProjectDependResolver
{
    public enum RegType
    {
          baseReg  = 10000
        , common = 20000
        , pre_services = 30000
        , edits = 40000
        , rules = 50000
        , filters =  60000
        , handlers  = 70000
         ,validations = 80000
        , services = 90000
        , post_services = 100000
        , set_Interface_defaults  = 101000
        , other  = int.MaxValue-1
        , test   = int.MaxValue
        
    }
    
    public interface IDependencyResolver{
    }

    public interface IDependencyRegistrar : IDependencyResolver
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        void Register(ContainerBuilder builder
                        ,IContainer Container
                        ,ITypeFinder typeFinder
                        ,IConfigurationSectionHandler config
                        ,IConfigOptions options);

        void ApplyAdditionRuleToParent(IEnumerable<IDependencyRegistrar> dr
                                      ,IConfigurationSectionHandler Config
                                      ,IConfigOptions options);


        bool ResolveDependencyName { set; get; }

        bool IsAssemblyValid(dynamic options);

        IEnumerable<string> IgnoreAssemblies { set; get; }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        int Order { get; }

        RegType RegisterType { get;}


    }
}
