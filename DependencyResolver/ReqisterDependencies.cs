using Autofac;
using DependencyResolver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectDependResolver
{
    public static class ReqisterDependencies
    {
        public static void DependencyRegister(dynamic container,dynamic Config, IConfigOptions options)
        {
            var typeFinder = new WebAppTypeFinder();
            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>().AsEnumerable();
            var drInstances = new List<IDependencyRegistrar>();

            foreach (var drType in drTypes)
            {
                var dependency = (IDependencyRegistrar)Activator.CreateInstance(drType);
                dependency.IsAssemblyValid(Config);
                drInstances.Add(dependency);
            }
                
            //sort
            foreach (var regtype in Enum.GetValues(typeof(RegType)))
            {
                var byRegTypeInstances = drInstances
                             .Where(w => w.RegisterType.ToString() == regtype.ToString())
                             .OrderBy(o => o.Order);

                foreach (var dr in byRegTypeInstances)
                {
                   var builder = new ContainerBuilder();
                    if (options.Contains(enumConfigOpts.RegAll))
                        dr.ResolveDependencyName = true;
                    dr.Register(builder, container, typeFinder, Config, options);
                    builder.Update(container);
                    dr.ApplyAdditionRuleToParent(byRegTypeInstances, Config, options);
                }
}
        }
    }
}
