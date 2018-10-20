using ReposDomain;
using ReposDomainRules;
using Core.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ReposServices.Extension
{
    public static class ServiceExtensions
    {
        public static IEnumerable<string> ExecuteRules(this BaseEntity baseEntity)
        {

            // var errors = baseEntity.RunDomainFunc;
            var RuleName = string.Format("{0}Rules", baseEntity.GetType().Name);
            var errors = new List<string>();

            var Rule=  EngineContext.Current
                .ContainerManager
                .Resolve<IServiceEntityRule>(key:RuleName, AllowNull:true);
            
            if (Rule !=null)
                errors= Rule.RunRules(baseEntity).ToList();
           
            return errors;
        }
    }
}
