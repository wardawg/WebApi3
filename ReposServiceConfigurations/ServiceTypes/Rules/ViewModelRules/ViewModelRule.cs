using Repos.DomainModel.Interface.DomainViewModels;
using Repos.DomainModel.Interface.Interfaces;
using ReposCore.Infrastructure;
using ReposServiceConfigurations.Common;
using System;
namespace ReposServiceConfigurations.ServiceTypes.Rules
{


    /// <summary>
    /// DomainRule
    /// Handles Client/Users
    /// Domain Rules
    /// </summary>
    public class DomainRule : IRule
    {
        public IClientInfo Client => new DefaultClientInfo();

        public IEntityRule CreateRule<D,M>()
        {

            return EngineContext
                   .Current
                   .ContainerManager
                   .Resolve<IEntityRule<D,M>>() as IEntityRule<D,M>;
        }

        public IEntityRule GetDomainRule(IBaseEntity baseEntity, IClientInfo clientInfo)
        {
            return baseEntity == null ? null: GetDomainRule(baseEntity.GetType(), clientInfo);
        }

        public IEntityRule GetDomainRule(Type t, IClientInfo clientInfo)
        {

            IEntityRule ret = default(IEntityRule);

            string ClientPrefix = clientInfo.AssmPrefix;
            string DefaultPrefix = clientInfo.DefaultPrefix;
            string ClientRuleName = string.Format("{0}.Common.{1}Rules", clientInfo.AssmPrefix, t.Name);
            string DefaultRuleName = string.Format("Repos.Common.{0}Rules", t.Name);
            string RuleName = string.Format("{0}Rules", t.Name);

            
            foreach (var strRule in new string[] { ClientRuleName, DefaultRuleName , RuleName })
            {
                ret = GetDomainRule(strRule);
                if (ret != null)
                    break;
            }


            //if (!String.IsNullOrEmpty(clientInfo.AssmPrefix) )
            //    ruleName = string.Format("{0}.{1}Rules", clientInfo.AssmPrefix, t.Name);
            //     ret = EngineContext
            //        .Current
            //        .ContainerManager
            //        .Resolve<IEntityRule>(key: ruleName, AllowNull: true);   
            //    if (ret == null)
            //    ruleName = string.Format("Repos.Common.{0}Rules", t.Name);
            //else
            //    ruleName = string.Format("Repos.Common.{0}Rules", t.Name);


                 if (ret == null)
                 {
                       throw new NotImplementedException("Rules For: " + t.Name + " is not implemented");
                 }
            return ret;
            
        }

        public IEntityRule GetDomainRule(string RuleName)
        {
           // var typeNameResolve = CommonUtil.GetResolveName(typeof(T), RuleName);

            var RulesType = EngineContext
                    .Current
                    .ContainerManager
                    .Resolve<IEntityRule>( key: RuleName, AllowNull:true);

        
       //     if (RulesType == null)
       //     {
       //         throw new NotImplementedException("Rules For: " + RuleName + " is not implemented");
       //     }

              return RulesType;
        }

        public IEntityRule GetViewModelRule(IDomainViewModel viewModel)
        {
            return viewModel == null ? null : GetDomainRule(viewModel.GetType().Name);

        }

        public IEntityRule GetViewModelRule(string viewModelName)
        {
           
            var RulesType = EngineContext
                        .Current
                        .ContainerManager
                        .Resolve<IEntityRule>(key: viewModelName, AllowNull: true);


            if (RulesType == null)
            {
               throw new NotImplementedException("Rules For: " + viewModelName + " is not implemented");
            }

            return RulesType;
            
        }

        public IEntityRule CreateRule<T>()
        {
            throw new NotImplementedException();
        }

        
    }
}
