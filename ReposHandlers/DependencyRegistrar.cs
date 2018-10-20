using Autofac;
using DependencyResolver;
using ProjectDependResolver;
using Repos.DomainModel.Interface.Atrributes.ServiceAttributes;
using Repos.DomainModel.Interface.Filters;
using Repos.DomainModel.Interface.Interfaces;
using ReposDomain.Handlers.Handlers.Common;
using ReposServiceConfigurations;
using ReposServiceConfigurations.ServiceTypes.Enums;
using ReposServiceConfigurations.ServiceTypesDependencies;
using System;
using System.Configuration;
using System.Linq;

namespace ReposDomain.Handlers
{
    public class DependencyRegistrar 
        : HandlerDepenciesRegistar
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


            base.Register(builder, Container, typeFinder, config, options);

            //ValidAssmPrefix(config);

            //if (!CallBaseResolver)
            //    return;

            //var RegTypes = ResolveTypes<IHandler>(typeFinder, options);



            //if (!Container.IsRegistered<ClientRefInfoHandler>())
            // builder
            //   .RegisterType<ClientRefInfoHandler>()
            //   .As<IClientRefInfoHandler>()
            //   .InstancePerLifetimeScope();

            //var sup = RegTypes
            //           .Where(w => !w.GetCustomAttributes(typeof(ServiceNoResolveAttribute), true)
            //                               .Any()).ToList<Type>();


            //SetDependency<IEditFilter, INullResolver>(
            //    builder
            //    , Container
            //     , sup
            //     , EnumServiceTypes.None
            //     , options
            //   );

            //SetDependency<IHandler, IServiceHandler, INullResolver>(
            //    builder
            //    , Container
            //     , sup
            //     , EnumServiceTypes.Handlers //, "Handlers"
            //     , options

            //   );

            //SetDependency<IGenericHandler, INullResolver>(
            //    builder
            //    , Container
            //     , sup
            //     , EnumServiceTypes.None
            //     , options
            //   );

        }

        private Type GetTargetInterface(Type t)
        {
            
            Type ret = t.GetInterfaces()
                 .Where(wx => wx.GetInterfaces()
                   .Any(ww => ww.GetInterfaces()
                     .Any(a => a.IsAssignableFrom(typeof(IGenericHandler)))))
                 .FirstOrDefault() ?? t.GetInterfaces().First();
            
            return ret;

        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public override int Order
        {
            get { return int.MaxValue; }
        }

        protected override bool CallBaseResolver { get; set; } = true;

        public override RegType RegisterType { get { return RegType.handlers; } }
        
    }
}
