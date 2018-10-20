using Autofac;
using DependencyResolver;
using ProjectDependResolver;
using ReposServiceConfigurations;
using ReposServiceConfigurations.ServiceTypes.Enums;
using ReposServiceConfigurations.ServiceTypes.Base;
using System.Configuration;
using System.Linq;

namespace RepoServices
{
    public class DependencyRegistrar 
        : ServiceDependencyRegister
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

                        

            if (!CallBaseResolver)
                return;


            var RegTypes = ResolveTypes<IBaseService>(typeFinder, options);

            SetDependency<IBaseService, INullResolver, INullResolver>(
              builder
              , Container
               , RegTypes
               , EnumServiceTypes.None //, "Handlers"
               , options

             );


            //var services = this.GetType().Assembly.GetTypes()
            //              .Where(t => typeof(IBaseService).IsAssignableFrom(t)
            //                          && !t.IsAbstract)
            //              .Where(i => i.GetInterfaces()
            //              .Any(g => g.IsGenericType
            //                    && g.GetType().GetInterfaces().Any()))
            //              .Select(map => new
            //              {
            //                  target = map
            //              });


            //ICacheService
            //foreach (var service in services)
            //{
            //    var serviceName = SetResolveName(service.target);

            //    if (!Container.IsRegisteredWithName<IBaseService>(serviceName))
            //        builder
            //            .RegisterType(service.target)
            //            .Named<IBaseService>(serviceName)
            //            .AsImplementedInterfaces()
            //            .InstancePerDependency();
            //}
            
        }


        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public override int Order
        {
            get { return 2; }
        }

        public override RegType RegisterType { get { return RegType.services; } }

        public override bool IsAssemblyValid(dynamic config)
        {
            return true;
        }
        protected override bool CallBaseResolver { get; set; } = true;
    }
}
