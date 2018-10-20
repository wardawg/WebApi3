using Repos.DomainModel.Interface.Interfaces;
using System.Collections.Generic;

namespace ReposCore.Custom.Types
{
    public struct EntityRules
    {
        public IDictionary<string, IBaseEntity> Rules;
        public EntityRules(dynamic Initializer)
       {
            //   var c = default(int);
            
           this.Rules = new Dictionary<string, IBaseEntity>();
        }

    }
 
}
