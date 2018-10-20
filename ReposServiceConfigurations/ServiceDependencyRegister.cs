#if DI_UNITY
#else
using Autofac;
#endif

using DependencyResolver;
using ProjectDependResolver;
using ReposServiceConfigurations.Common;
using ReposServiceConfigurations.ServiceTypes.Attributes;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServiceConfigurations.ServiceTypes.Edits;
using ReposServiceConfigurations.ServiceTypes.Enums;
using ReposServiceConfigurations.ServiceTypes.Rules;
using ReposServiceConfigurations.ServiceTypes.Rules.DomainRules;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using static ReposServiceConfigurations.Common.CommonUtil;

namespace ReposServiceConfigurations
{

    public interface INullResolver{
    }

    public class PreServiceDependencyRegistrar : ServiceDependencyRegister
    {
            
      

        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public override void Register(

#if DI_UNITY
#else
            ContainerBuilder builder
          , IContainer Container
#endif
         , ITypeFinder typeFinder
         , IConfigurationSectionHandler config
         , IConfigOptions options)
        {




           // var appConfig = ConfigurationManager.OpenExeConfiguration("C:\\VSProjects\\Repos\\bin\\ReposServiceConfigures.dll");

           // var local = appConfig.Sections.Cast<ConfigurationSection>().Where(s => s.SectionInformation.Name == "ServiceConfig");
           var appConfig = ConfigurationManager.OpenExeConfiguration(this.GetType().Assembly.Location);

            //Assembly.GetExecutingAssembly().Location);

            var localconfig = appConfig.GetSection("ServiceConfig") as IConfigurationSectionHandler;


            //if (options.Contains( enumConfigOpts.RegAll))
            //    SetResolveNameFlag(ResolveDepName.YES);
            
        }
        

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public override int Order
        {
            get { return 0; }
        }

        public override RegType RegisterType { get { return RegType.pre_services; } }

        public override bool IsAssemblyValid(dynamic config)
        {
            return true;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class ServiceDependencyRegister : BaseDependencyRegistrar
    {
        
        public ServiceDependencyRegister()
        {
            IgnoreAssemblies = "Test|RepoUnitTest".Split('|');
        }

        public override void Register(ContainerBuilder builder
                                    , IContainer Container
                                    , ITypeFinder typeFinder
                                    , IConfigurationSectionHandler config
                                    , IConfigOptions options){
        }

        public override bool IsAssemblyValid(dynamic config)
        {
            ValidAssmPrefix(config);
            return true;
        }

               
        protected virtual void SetDependency<TResolvingInterface, TGenericInterface>
            (ContainerBuilder builder
            , IContainer Container
            , IList<Type> DepTypes
            , EnumServiceTypes sPosfix
            , IConfigOptions opts
            )
        {
            
            SetDependency<TResolvingInterface, TResolvingInterface, TGenericInterface>
              (builder
              , Container
              , DepTypes
              , sPosfix
              , opts
              );

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNamedInterface"></typeparam>
        /// <typeparam name="TResolvingInterface"></typeparam>
        /// <typeparam name="TGenericInterface"></typeparam>
        /// <param name="builder"></param>
        /// <param name="Container"></param>
        /// <param name="DepTypes"></param>
        /// <param name="sPosfix"></param>
        protected virtual void SetDependency<TNamedInterface
                                    , TResolvingInterface
                                    , TGenericInterface>
            (
            
              ContainerBuilder builder
            , IContainer Container
            , IList<Type> DepTypes
            , EnumServiceTypes sPosfix
            , IConfigOptions options
            )
        {

            

            var assm = String.Join("|", IgnoreAssemblies) + "Unknown"; //needed for test    ing



            var regtype = DepTypes
                          .Where(t => (typeof(TResolvingInterface).IsAssignableFrom(t)
                                        || IsAssignableFrom(t, typeof(TNamedInterface)))
                                && !(t.IsInterface
                                      || t.IsGenericType
                                      || t.IsAbstract
                                      || t.IsSealed
                                     )
                                 && ! t.GetCustomAttributes(typeof(NoServiceResolveAtrribute), true)
                                       .Any()
                                 && !Regex.IsMatch(t.Assembly.FullName, assm))
                          .ToList();


            regtype.Select(map => new
            {
                target = map
               ,source = GetGenericInterfaceType<TGenericInterface>(map)

            })
              .ToList()
              .ForEach(f => {

                  string infaceName = String.Empty;
                  var intfaceName = f
                                    .target
                                    .GetInterfaces()
                                    .Where(g => g.Name.ToLower().Substring(1)
                                                    == f.target.Name.ToLower())
                                                .FirstOrDefault();
                    if (intfaceName != null)
                      infaceName =intfaceName.Name.Substring(1);

                      var name = CommonUtil
                                .GetResolveName(f.target
                                                , Name: infaceName
                                                , postFix: sPosfix
                                                , Opts: options);

#if DI_UNITY

                  if (!Container.IsRegistered<TNamedInterface>(name))
                      Container
                      .RegisterType(f.source,f.target, name);
#else
                  if (!Container.IsRegisteredWithName<TNamedInterface>(name))
                      BuildAndScope<TNamedInterface>(builder,f.target,name);
                  }
               );
        }
#endif

#if DI_UNITY
        protected virtual void BuildAndScope(IContainer Container,Type Source, Type Target,string Name)
        {
            Container
                    .RegisterType(Source, Target, Name);
        }
#endif

        private void BuildAndScope<TNamedInterface>(ContainerBuilder builder
                                             ,Type type
                                             , string name
                                             )
        {

            BuildScope<TNamedInterface>(builder, type, name);
        }

       
        protected virtual void BuildScope<TNamedInterface>(ContainerBuilder builder
                                             , Type regtype
                                             , string name)
                                             
        {
            builder
           .RegisterType(regtype)
           .Named<TNamedInterface>(name)
           .AsImplementedInterfaces()
           .InstancePerDependency();
        }


        protected virtual IList<Type> ResolveTypes<TInterface>(ITypeFinder typeFinder
                                                              ,IConfigOptions options)
        {
            IList<Type> RegTypes = new List<Type>();

          //  if (options.Contains(enumConfigOpts.RegAll))
                RegTypes = typeFinder
                            .FindClassesOfType<TInterface>()
                            .Where(w=>!( w.IsAbstract || w.IsInterface))
                            .ToList();
            //else
            //    RegTypes = this.GetType()
            //                   .Assembly
            //                   .GetTypes()
            //                   .ToList<Type>();
            return RegTypes;
        }


        protected virtual Type GetGenericInterfaceType<T>(Type t)
        {
            Type ret = default(Type);

            if (t.GetInterfaces().Any())
            {
                try
                {

#if DI_UNITY
                    ret = t.GetInterfaces()
                        .Where(w => !w.IsGenericType
                                    && typeof(T)
                                        .IsAssignableFrom(w))
                                     .FirstOrDefault();
#else
                    ret = t.GetInterfaces()
                         .Where(w => typeof(T).IsAssignableFrom(w)
                          && w.IsGenericType &&
                          w.GetGenericArguments()
                              .Any(a => a.GetInterfaces().Any(aa => aa.IsGenericType)))
                              .Where(ww => ww.GetGenericArguments().Any())
                          .Select(s => s.GetGenericArguments().FirstOrDefault())
                          .FirstOrDefault();
#endif
                }
                catch
                {
                    System.Diagnostics.Debug.Write("");
                }
            }

            return ret;

        }

        private bool IsAssignableFrom(Type t, Type i)
        {
            bool res = false;

            if (t.GetInterfaces().Any())
                foreach (var ix in t.GetInterfaces())
                {
                    res = IsAssignableFrom(ix, i);
                    if (res)
                        break;
                }

            res = t.GetInterfaces()
                  .Any(a => i.IsAssignableFrom(a));

            return res;
        }

        public override int Order => int.MaxValue - 1;

    }
    

    /// <summary>
    /// 
    /// </summary>
    public class PostServiceDependencyRegistrar : ServiceDependencyRegister
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

            if (!Container.IsRegistered<IRule>())
                builder
                .RegisterType<DefaultDomainRule>()
                .As<IRule>()
                .InstancePerLifetimeScope();

            if (!Container.IsRegistered<IDomainEdit>())
                builder
                  .RegisterType<DefaultServiceEntityEdit>()
                  .As<IDomainEdit>()
                  .InstancePerLifetimeScope();

            if (!Container.IsRegistered<ICacheService>())
                builder
                .RegisterType<CacheService>()
                .As<ICacheService>()
                .InstancePerLifetimeScope();
        }
        

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public override int Order
        {
            get { return int.MaxValue; }
        }

        public override RegType RegisterType { get { return RegType.post_services; } }


        public override bool IsAssemblyValid(dynamic config)
        {
            return true;
        }
    }
}
