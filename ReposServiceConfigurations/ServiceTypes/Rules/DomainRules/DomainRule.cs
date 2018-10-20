using Repos.DomainModel.Interface.DomainViewModels;
using Repos.DomainModel.Interface.Interfaces;
using ReposCore.Infrastructure;
using ReposServiceConfigurations.Common;
using ReposServiceConfigurations.ServiceTypes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReposServiceConfigurations.ServiceTypes.Rules
{


    /// <summary>
    /// DomainRule
    /// Handles Client/Users
    /// Domain Rules
    /// </summary>
    public sealed class DomainRule : IRule
    {
        public IClientInfo Client => new DefaultClientInfo();
             

        public IEntityRule CreateRule<D,M>()
        {

            return EngineContext
                   .Current
                   .ContainerManager
                   .Resolve<IEntityRule<D,M>>() as IEntityRule<D,M>;
        }

        public IEnumerable<IEntityRule> GetDomainRules(IBaseEntity baseEntity, IClientInfo clientInfo)
        {
            return baseEntity == null ? new List<IEntityRule>(): GetDomainRules(baseEntity.GetType(), clientInfo);
        }

        private void GetClientInfo(Type t
                                  , IClientInfo clientInfo
                                   , out string  ClientPrefix
                                  , out string DefaultPrefix
                                  , out string ClientRuleName
                                  , out string DefaultRuleName
                                  , out string RuleName)
        {

            ClientPrefix = clientInfo.AssmPrefix;
            DefaultPrefix = clientInfo.DefaultPrefix;
            ClientRuleName = string.Format("{0}.{2}.{1}{2}", clientInfo.AssmPrefix, t.Name, EnumServiceTypes.Rules);
            DefaultRuleName = string.Format("Repos.{0}.{1}{2}", EnumServiceTypes.Rules, t.Name, EnumServiceTypes.Rules);
            RuleName = string.Format("{0}{1}", t.Name, EnumServiceTypes.Rules);
            
        }

        public IEnumerable<IEntityRule> GetDomainRules(Type t
                                                        , IClientInfo clientInfo
                                                        , string[] sRules = null)
        {

            string ClientPrefix = string.Empty;
            string DefaultPrefix = string.Empty;
            string ClientRuleName = string.Empty;
            string DefaultRuleName = string.Empty;
            string RuleName = string.Empty;

            List<IEntityRule> rules = new List<IEntityRule>();
            if (sRules == null)
            {

                GetClientInfo(t
                            ,clientInfo
                            , out ClientPrefix
                            , out DefaultPrefix
                            , out ClientRuleName
                            , out DefaultRuleName
                            , out RuleName
                            );
            }

            foreach (var strRule in sRules ?? new string[] { ClientRuleName, DefaultRuleName, RuleName })
            {
               var  rule = GetDomainRule(strRule);
                if (rule != null)
                    rules.Add(rule);
                    //break;
            }

            //if (!rules.Any())
            //{
            //    throw new NotImplementedException("Rules For: " + t.Name + " is not implemented");
            //}
            return rules;
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

        public IEntityRule GetDomainRule(IBaseEntity baseEntity, IClientInfo clientInfo)
        {
            throw new NotImplementedException();
        }

        public IEntityRule GetDomainRule(Type t, IClientInfo clientInfo)
        {
            string ClientPrefix = string.Empty;
            string DefaultPrefix = string.Empty;
            string ClientRuleName = string.Empty;
            string DefaultRuleName = string.Empty;
            string RuleName = string.Empty;
            IEntityRule ret = default(IEntityRule);

            GetClientInfo(t
                            ,clientInfo
                            , out ClientPrefix
                            , out DefaultPrefix
                            , out ClientRuleName
                            , out DefaultRuleName
                            , out RuleName
                );

           ret = GetDomainRules(t
                            , clientInfo
                            , new string[] { ClientRuleName , DefaultRuleName }).FirstOrDefault();
            
            return ret;
        }

        //public IEnumerable<IEntityRule> GetDomainRules(Type t, IClientInfo clientInfo)
        //{

        //    return GetDomainRules(t,clientInfo,)
        //    return new List<IEntityRule>(); 
        //}
    }
}
