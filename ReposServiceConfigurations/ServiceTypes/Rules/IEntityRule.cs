using Repos.DomainModel.Interface.Interfaces;
using ReposCore.Custom.Types;
using System.Web.Mvc;

namespace ReposServiceConfigurations.ServiceTypes.Rules
{

    public interface IEntityRule
        : IModelRule
    {

        void RunRules(IBaseEntity Entity
               , EntityRules Entities
               , ModelStateDictionary modelState);

        void RunRulesOnModel(IViewModel Entity
                           , ModelStateDictionary modelState
                            );

        bool Required { get; set; }

        bool IsBaseRule { get; }
    }

    public interface IEntityRule<T> : IEntityRule
    {
       

    }
    public interface IEntityRule<D,M> :
         IEntityRule<D>
        ,IEntityRule
    {   
       
    }
}
