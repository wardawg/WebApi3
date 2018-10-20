
using Repos.DomainModel.Interface.DomainViewModels;
using Repos.DomainModel.Interface.Interfaces;
using System;
using System.Collections.Generic;

namespace ReposServiceConfigurations.ServiceTypes.Rules
{
    public interface IRule
    {

        IEnumerable<IEntityRule> GetDomainRules(IBaseEntity baseEntity, IClientInfo clientInfo);
        IEnumerable<IEntityRule> GetDomainRules(Type t, IClientInfo clientInfo,string[] sRule = null);

        IEntityRule GetDomainRule(IBaseEntity baseEntity
                                  , IClientInfo clientInfo
                                  );

        IEntityRule GetDomainRule(Type t, IClientInfo clientInfo);
               

        IEntityRule CreateRule<D,M>();
        IEntityRule GetViewModelRule(IDomainViewModel ViewModelEntity);

        IClientInfo Client { get; }

        
    }
}