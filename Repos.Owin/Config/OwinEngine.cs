using Autofac;
using ProjectDependResolver;
using ReposCore.Infrastructure;
using ReposCore.Infrastructure.DependencyManagement;
using System;
using System.Collections.Generic;
using System.Configuration;


namespace Repos.Owin.Config
{
    public class OwinEngine : IEngine
    {
        private ContainerManager _containerManager;

        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }


        public void Initialize(IConfigurationSectionHandler config)
        {
            RegisterDependencies(config);
        }
        protected virtual void RegisterDependencies(IConfigurationSectionHandler config)
        {
           
            var builder = new ContainerBuilder();
            var container = builder.Build();
            this._containerManager = new ContainerManager(container);
          
                //dependencies
             var typeFinder = new WebAppTypeFinder();

            builder = new ContainerBuilder(); 
           builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
           builder.Update(container);
    


            //foreach (var dependencyRegistrar
            //               in new Type[]{
            //                   typeof(DependencyRegistrar)
            //                  ,typeof(Repos.DomainModel.Interface.DependencyRegistrar)
            //                  ,typeof(ReposDomain.Handlers.DependencyRegistrar)
            //               })
              
            //{
            //    var dep = (IDependencyRegistrar)Activator.CreateInstance(dependencyRegistrar);
            //    builder = new ContainerBuilder();
            //    dep.Register(builder, container, null, config, new DefaultConfOptions());
            //    builder.Update(container);
            //}

            ReqisterDependencies.DependencyRegister(container, config, new DefaultConfOptions());


        }

        public object Resolve(Type type)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(bool AllowNull = false) where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            throw new NotImplementedException();
        }
    }
}
