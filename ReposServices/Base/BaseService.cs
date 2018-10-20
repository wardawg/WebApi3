using Repos.DomainModel.Interface.Interfaces;
using ReposCore.Caching;
using ReposCore.Custom.Types;
using ReposCore.Extensions;
using ReposCore.FunctionInterfaces;
using ReposCore.Infrastructure;
using ReposData.Repository;
using ReposDomain.Extentions.Extensions;
using ReposServiceConfigurations.Common;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServiceConfigurations.ServiceTypes.Edits;
using ReposServiceConfigurations.ServiceTypes.Enums;
using ReposServiceConfigurations.ServiceTypes.Rules;
using ReposServiceConfigures.ServiceTypes.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ReposServices.Base
{
    public abstract class BaseService<T>
        : IBaseService<T> where T : BaseEntity<T>
    {
        protected IRepository<T> _Repos;
        protected IDomainEdit _Edits;
        protected IRule _Rules;
        static IClientInfo _clientInfo = null;
        private ICacheManager _Cache;

        private IServiceEntityEdit<T> _EntityEdit 
               = default(IServiceEntityEdit<T>);

        ServiceRuleFunc<bool> RuleFuncCall
             = new ServiceRuleFunc<bool>((Entities, ModelState, RuleFactory) =>
                 RunEntityRules((EntityRules)Entities
                                , (ModelStateDictionary)ModelState
                                , (IRule)RuleFactory));


        /// <summary>
        /// BaseService 
        /// Base for all repositories
        /// </summary>
        /// <param name="Repos"></param>
        /// <param name="Edits"></param>
        /// <param name="Rules"></param>
        protected BaseService(IRepository<T> Repos
                            , ICacheService Cache
                            , IDomainEdit Edits
                            , IRule Rules
                           // , IUser User
                             )
        {
            this._Repos = Repos;
            this._Edits = Edits;
            this._Rules = Rules;
            this._Cache = Cache;

            if (_Rules == null)
                RulesEnabled = false;

            InitilizeServicesModules();
        }

        public ICacheManager Cache => _Cache;

        public bool LoadEntityDefaults { set; get; } = true;
        public bool RulesEnabled { set; get; } = true;

        public virtual void InitilizeServicesModules(){
        }

        
        /// <summary>
        /// Create business objects 
        /// thru services only
        /// </summary>
        /// <returns></returns>
        protected T CreateEntity()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }

        public T CreateServiceEntity(CreateEntityOptions createEntityOptions)
        {
            return CreateServiceEntityWithOptions();
        }

        public virtual T CreateServiceEntityWithOptions()
        {

            var Entity = CreateEntity();
            if (_Edits != null)
            {
                var EntityEdit = _Edits.CreateEdit<T>();
                if (EntityEdit != null)
                    EntityEdit.SetEntityValues(Entity);
            }

            return Entity;
        }
        public virtual T CreateServiceEntity()  
        {
            var Entity = CreateEntity();

            if (LoadEntityDefaults && _Edits != null)
            {
                var EntityEdit = _Edits.CreateEdit<T>(); 
                if (EntityEdit != null)
                    EntityEdit.SetEntityDefaults(Entity);
            }
               
            return Entity;
        }

        public T CreateServiceEntity(IClientInfo clientInfo)
        {

            var Entity = CreateEntity();

            string ClientPrefix = clientInfo.AssmPrefix;
            //string DefaultPrefix = clientInfo.DefaultPrefix;
            string ClientEditName = string.Format("{0}.{2}.{1}{2}", clientInfo.AssmPrefix, typeof(T).Name, EnumServiceTypes.Edits);
            string DefaultEditName = string.Format("Repos.Common.{0}{1}", typeof(T).Name, EnumServiceTypes.Edits);
                        

            // if (clientInfo.AssmPrefix != clientInfo.DefaultPrefix)
            //ResolvefilterName = string.Format("{0}.{1}.", ClientPrefix,EnumServiceTypes.Edits) ;

            var edit = GetEditByName(ClientEditName) ?? GetEditByName(DefaultEditName);
            
            if (edit != null)
                edit.SetEntityDefaults(Entity);

            return Entity;
                       
        }

        private static IServiceEntityEdit<T> GetEditByName(string editName)
        {
            return EngineContext
                  .Current
                  .ContainerManager
                  .Resolve<IDomainEdit>(editName,AllowNull:true) as IServiceEntityEdit<T>; 

        }

        public IQueryable<T> Table { get; }
        protected IQueryable<T> TableNoTracking { get; }
        public virtual void Add(T entity)
        {
            if (entity == null)
                throw new NullReferenceException("entity Add");

            this.RulesEnabled = entity.RulesEnabled;
            _Repos.Add(entity);
        }
        public virtual void Add(IEnumerable<T> entities)
        {
            _Repos.Add(entities);
        }
        public virtual void Update(T entity)
        {
             _Repos.Update(entity);
        }

        public virtual void Update(T entity,IClientInfo client)
        {
            _clientInfo = client;
            _Repos.Update(entity);
        }

        public virtual void Update(IEnumerable<T> entities)
        {
            _Repos.Update(entities);
        }
        public virtual Result Delete(T entity)
        {
            return _Repos.Delete(entity);
        }

        public virtual Result Delete(IEnumerable<T> entities)
        {
            return _Repos.Delete(entities);
        }
        public virtual IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return  _Repos.TableNoTracking.Where(predicate);
        }
         


        /// <summary>
        /// Execute rule by client or user
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="factory"></param>
        /// <param name="Entities"></param>
        /// <param name="modelState"></param>
        private static void ExecuteRule(IBaseEntity Entity
                                       ,IRule Rulefactory
                                       ,EntityRules Entities
                                       ,ModelStateDictionary modelState)
        {
            var client = _clientInfo ?? Rulefactory.Client;

            var rules = Rulefactory.GetDomainRules(Entity, client);
                      
             foreach(var rule in rules)
            {
                if (rule.Required )
                    rule.RunRules(Entity, Entities, modelState);
            }
                
        }
        private static bool RunEntityRules(EntityRules Entities
                                          , ModelStateDictionary   modelState
                                          , IRule          RuleFac
                                          )
        {
            foreach (KeyValuePair<string, IBaseEntity> entity in Entities.Rules)
            {
                if (!entity.Value.RulesEnabled)
                    continue;

                ExecuteRule(entity.Value, RuleFac, Entities, modelState);
            }

            return modelState.IsValid;
                
        }
            
        public virtual T GetById(object Id)
        {
            var result = _Repos.GetById(Id);

            if (result != null && _EntityEdit != null)
            {
                _EntityEdit.SetEntityValues(result);
                result.SetCleanEntity();
                               
            }

            return result;
        }
              
       public virtual IQueryable<T> GetAll()
       {
         return _Repos.Table;
       }

        public Result Verify(ModelStateDictionary ModelState)
        {
            if (_Rules == null)
                RulesEnabled = false;

            var Func = RulesEnabled ? RuleFuncCall : null;
            return _Repos.Verify(Func, ModelState, _Rules);
        }
    

        public Result Verify(ModelStateDictionary ModelState, IClientInfo clientInfo)
        {
            _clientInfo = clientInfo;
            return Verify(ModelState);
            
        }

     
    }
}
