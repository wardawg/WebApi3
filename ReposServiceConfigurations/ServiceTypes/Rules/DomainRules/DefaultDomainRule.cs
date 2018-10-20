using Repos.DomainModel.Interface.DomainViewModels;
using Repos.DomainModel.Interface.Interfaces;
using ReposServiceConfigurations.Common;
using System;
using System.Collections.Generic;

namespace ReposServiceConfigurations.ServiceTypes.Rules.DomainRules
{
    public sealed class DefaultDomainRule : IDomainRule
    {


        public IClientInfo Client => new DefaultClientInfo();

      
        public IEntityRule CreateRule<T>()
        {
            return null;
        }

        public IEntityRule CreateRule<D, M>()
        {
            return null;
        }

        public IEntityRule GetDomainRule(IBaseEntity baseEntity, IClientInfo clientInfo)
        {
            throw new NotImplementedException();
        }

        public IEntityRule GetDomainRule(Type t, IClientInfo clientInfo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEntityRule> GetDomainRules(IBaseEntity baseEntity, IClientInfo clientInfo)
        {
            return null;
        }

        public IEnumerable<IEntityRule> GetDomainRules(Type t, IClientInfo clientInfo)
        {
            return null;
        }

        public IEnumerable<IEntityRule> GetDomainRules(Type t, IClientInfo clientInfo, string[] sRule = null)
        {
            throw new NotImplementedException();
        }

        public IEntityRule GetViewModelRule(IDomainViewModel ViewModelEntity)
        {
            return null;
        }
    }
}
