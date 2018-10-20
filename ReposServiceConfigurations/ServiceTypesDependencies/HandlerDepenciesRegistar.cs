using Autofac;
using DependencyResolver;
using ProjectDependResolver;
using Repos.DomainModel.Interface.Atrributes.ServiceAttributes;
using Repos.DomainModel.Interface.Filters;
using Repos.DomainModel.Interface.Interfaces;
using ReposServiceConfigurations.ServiceTypes.Enums;
using ReposServiceConfigures.ServiceTypes.Handlers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ReposServiceConfigurations.ServiceTypesDependencies
{
    public class HandlerDepenciesRegistar
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

            //  ValidAssmPrefix(config);

            if (!CallBaseResolver)
                return;
            

            var RegTypes = new IList<Type>[] {
                                 ResolveTypes<IHandler>(typeFinder, options)
                                 ,ResolveTypes<IEditFilter>(typeFinder, options)
                                }.SelectMany(s => s)
                                 .ToList();

            var sup = RegTypes
                     .Where(w => !w.GetCustomAttributes(typeof(ServiceNoResolveAttribute), true)
                                         .Any()).ToList<Type>();


            SetDependency<IEditFilter, INullResolver>(
                builder
                , Container
                 , sup
                 , EnumServiceTypes.None
                 , options
               );

            SetDependency<IHandler, IServiceHandler, INullResolver>(
                builder
                , Container
                 , sup
                 , EnumServiceTypes.Handlers //, "Handlers"
                 , options

               );

            SetDependency<IGenericHandler, INullResolver>(
                builder
                , Container
                 , sup
                 , EnumServiceTypes.None
                 , options
               );

            if (!Container.IsRegistered<IServiceHandlerFactory>())
                builder
                 .RegisterType<ServiceHandlerFactory>()
                 .As<IServiceHandlerFactory>()
                 .InstancePerLifetimeScope();


            if (!Container.IsRegistered(typeof(IGenericHandler<>)))
                builder
                  .RegisterGeneric(typeof(GenericReposHandler<>))
                  .As(typeof(IGenericHandler<>))
                  .InstancePerLifetimeScope();


            if (!Container.IsRegistered<IServiceGenericHandler>())
                builder
                    .RegisterType<GenericHandler>()
                    .As<IServiceGenericHandler>()
                    .InstancePerLifetimeScope();
                        
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

        public override bool IsAssemblyValid(dynamic config)
        {
            return true;
        }

       
        public override RegType RegisterType { get { return RegType.handlers; } }

    }
}
