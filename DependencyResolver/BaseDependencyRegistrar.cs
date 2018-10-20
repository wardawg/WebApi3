using Autofac;
using DependencyResolver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ProjectDependResolver
{


    public abstract class BaseDependencyRegistrar : IDependencyRegistrar
    {
        
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public abstract void Register(ContainerBuilder builder
          , IContainer Container
          , ITypeFinder typeFinder
          , IConfigurationSectionHandler config
          , IConfigOptions options);


        public bool ResolveDependencyName { set; get; } = false;

        protected virtual bool CallBaseResolver { set; get; } = true;
              

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public virtual int Order
        {
            get { return int.MaxValue; }
        }

        
        public string SetResolveName(Type t
                                    , string Name = ""
                                    , string postFix=""
                                    , IConfigOptions Opts = default(IConfigOptions))
        {
             string TypeName = t.Name;
            
            if (!ResolveDependencyName
                && (Opts != null && !Opts.Contains(enumConfigOpts.ForceNameResolve))
                )
                return TypeName;
            
            if (!string.IsNullOrEmpty(Name))
                TypeName = Name;
            
            string name;
           
            var temp = t.FullName.Split('.');
            if (!string.IsNullOrEmpty(postFix))
                name = temp.First();
            else
                name = string.Join(".", temp.Take(temp.Length-1));

            var sreturn = name + "." + (string.IsNullOrEmpty(postFix) ? "" :  postFix + ".") + TypeName;

            return sreturn;

        }

        public void ValidAssmPrefix(dynamic config)
        {
            bool result = true;

            if (!config.DLLValidation)
                return;


                string prefixes = config.RuntimePrefixes;

                var AssmName = this
                                .GetType()
                                .Assembly
                                .FullName
                                .Split(',')
                                .First();

                var AssmNode = this
                                .GetType()
                                .Assembly
                                .FullName
                                .Split(',')
                                .Where(f => f.Split('.').Count() > 1)
                                .DefaultIfEmpty(".")
                                .First()
                                .Split('.')
                                .Last();
               
                result = prefixes.Split(',').Contains(AssmNode);
                var err = String.Format("DLL Naming Convention {0} Is Not Valid In {1}"
                                        , AssmName
                                        , this.GetType().Assembly.FullName.Split(',').First());

                if (!result)
                    throw new Exception(err);
            
        }

        public virtual bool IsAssemblyValid(dynamic options)
        {
            return true;
        }

       public void ApplyAdditionRuleToParent(IEnumerable<IDependencyRegistrar> dr
                                           , IConfigurationSectionHandler Config
                                           , IConfigOptions options)
        {
            if(CallBaseResolver == false)
                foreach(var resolver in dr)
                {
                    if (resolver.GetType() != this.GetType())
                        if (resolver.GetType().IsAssignableFrom(this.GetType()))
                            ((BaseDependencyRegistrar)resolver).CallBaseResolver = false;
                }
        }
        
        public virtual RegType RegisterType { get { return RegType.baseReg; } }

        public virtual IEnumerable<string> IgnoreAssemblies { get; set; } 
    }
}
