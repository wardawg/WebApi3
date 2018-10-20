using DependencyResolver;

#if  DI_UNITY
using Unity;
using Unity.Lifetime;
using Unity.AspNet.WebApi;

#else
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.Wcf;
using Autofac.Integration.WebApi;

#endif

using ProjectDependResolver;
using ReposCore.Configuration;
using ReposCore.Infrastructure.DependencyManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace ReposCore.Infrastructure
{

    public class ReposEngine : IEngine
    {
        #region Fields

        private const string ResolverType = "Mvc";
        private ContainerManager _containerManager;

        #endregion

        #region Utilities

        /// <summary>
        /// Run startup tasks
        /// </summary>
        protected virtual void RunStartupTasks()
        {
            var typeFinder = _containerManager.Resolve<ITypeFinder>();
            var startUpTaskTypes = typeFinder.FindClassesOfType<IStartupTask>();
            var startUpTasks = new List<IStartupTask>();
            foreach (var startUpTaskType in startUpTaskTypes)
                startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
            //sort
            startUpTasks = startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();
            foreach (var startUpTask in startUpTasks)
                startUpTask.Execute();
        }

        /// <summary>
        /// Register dependencies
        /// </summary>
        /// <param name="config">Config</param>

#if DI_UNITY
        protected virtual void RegisterDependencies(ReposConfig config, IConfigOptions options)
        {
            
            var container = new UnityContainer();
            this._containerManager = new ContainerManager(container);

            //we create new instance of ContainerBuilder
            //because Build() or Update() method can only be called once on a ContainerBuilder.

            //dependencies
            var typeFinder = new WebAppTypeFinder();
            
            container.RegisterInstance<IConfigurationSectionHandler>(config,new SingletonLifetimeManager());
            container.RegisterInstance<IEngine>(this, new SingletonLifetimeManager());
            container.RegisterInstance<ITypeFinder>(typeFinder, new SingletonLifetimeManager());
            

            if (!config.DLLValidation)
                options.Remove(enumConfigOpts.NoDLLValidation);

            ReqisterDependencies.DependencyRegister(container, config, options);

          
            //set dependency resolver
            if (config.ResolverType == ResolverType)
            {
                var resolver = new UnityDependencyResolver(container);
                System.Web.Mvc.DependencyResolver.SetResolver(resolver);
            }
            else
            {
                var resolver = new UnityDependencyResolver(container);
                GlobalConfiguration.Configuration.DependencyResolver = resolver;
            }


        }
#else
        protected virtual void RegisterDependencies(ReposConfig config, IConfigOptions options)
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();
            this._containerManager = new ContainerManager(container);

            //we create new instance of ContainerBuilder
            //because Build() or Update() method can only be called once on a ContainerBuilder.

            //dependencies
            var typeFinder = new WebAppTypeFinder();
            builder = new ContainerBuilder(); 
            builder.RegisterInstance(config).As<ReposConfig>().SingleInstance();
            builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            builder.Update(container);

            if (!config.DLLValidation)
                options.Remove(enumConfigOpts.NoDLLValidation);

            ReqisterDependencies.DependencyRegister(container,config, options);
                      
            builder = new ContainerBuilder();

            var assm = typeFinder.App.GetAssemblies().ToArray();
                
             //set dependency resolver
            if (config.ResolverType == ResolverType )
            {
                builder.RegisterControllers(assm);
                builder.Update(container);
                // 10/27 somehow this broke 
                // and had to use the full declaration
                System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

                
            }
            else if (config.ResolverType == "WebWcf")
            {
                AutofacHostFactory.Container = container;
            }
            else
            {
                builder.RegisterApiControllers(assm);
                builder.Update(container);
                var webApiResolver = new AutofacWebApiDependencyResolver(container);
                GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;
            }

            
        }
#endif
        #endregion

#if DI_UNITY
        #region Methods

        /// <summary>
        /// Initialize components and plugins in the nop environment.
        /// </summary>
        /// <param name="config">Config</param>
        public void Initialize(IConfigurationSectionHandler configHandler)
        {

            ConfOptions options = new DefaultConfOptions();


            //register dependencies
            RegisterDependencies((ReposConfig)configHandler, options);
            RunStartupTasks();
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T Resolve<T>(bool AllowNull = false) where T : class
        {
            return  ContainerManager.Resolve<T>(AllowNull: AllowNull);
        }

        /// <summary>
        ///  Resolve dependency
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return ContainerManager.Container.Resolve(type);
        }

        /// <summary>
        /// Resolve dependencies
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return ContainerManager.Container.ResolveAll<T>();
        }



        #endregion

        #region Properties

        /// <summary>
        /// Container manager
        /// </summary>
        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }
        #endregion
#else
        #region Methods

        /// <summary>
        /// Initialize components and plugins in the nop environment.
        /// </summary>
        /// <param name="config">Config</param>
        public void Initialize(IConfigurationSectionHandler configHandler)
        {

            ConfOptions options = new DefaultConfOptions();
                        

            //register dependencies
            RegisterDependencies((ReposConfig)configHandler, options);
            RunStartupTasks();
         }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T Resolve<T>(bool AllowNull = false) where T : class
        {
            return ContainerManager.Resolve<T>(AllowNull: AllowNull);
        }

        /// <summary>
        ///  Resolve dependency
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        /// <summary>
        /// Resolve dependencies
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

     

        #endregion

        #region Properties

        /// <summary>
        /// Container manager
        /// </summary>
        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }


        #endregion
#endif
    }


}
