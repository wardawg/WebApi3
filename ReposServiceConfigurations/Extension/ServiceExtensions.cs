using Repos.DomainModel.Interface.Atrributes.DynamicAttributes;
using Repos.DomainModel.Interface.DomainComplexTypes;
using Repos.DomainModel.Interface.DomainViewModels;
using Repos.DomainModel.Interface.Filters;
using Repos.DomainModel.Interface.Interfaces;
using Repos.DomainModel.Interface.Interfaces.Filter;
using ReposCore.Infrastructure;
using ReposServiceConfiguration.ServiceTypes.Base;
using ReposServiceConfigurations.Common;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServiceConfigurations.ServiceTypes.Enums;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace ReposServiceConfigurations.Extensions
{
    public static class ServiceExtensions
    {

        private static IEngine HandlerResolve = EngineContext.Current;


        public static IEnumerable<T> GetApiVersion<T>(this IQueryable<T> api
                                                 , Expression<Func<T, bool>> predicate = null)
        {

            Func<T, bool> deg = i => true;
            Expression<Func<T, bool>> expr = i => true;
            var p = predicate == null ? expr : predicate;

            
            return api
                   .Where(p)
                   .ToList();
            
        }

        public struct PropKeyPairFieldDef
        {
            public string Key;
            public string Value;
            public IBaseEntity Entity;

            public PropKeyPairFieldDef(IBaseEntity Entity
                                    ,string Key
                                    , string Value
                                    ) 
                : this()
            {
                this.Key = Key;
                this.Value = Value;
                this.Entity = Entity;
            }
        }

      

        public static IEnumerable<IFilterKeyPair> GetValue(this IEnumerable<IFilterKeyPair> filters)
        {
            return filters; 
        }

        public static IEnumerable<IFilterKeyPair> GetValue(this IFilterKeyPair filter
                                                          , DomainEntityType Entity)
        {
            return filter.GetFilterValues(Entity);
        }


        public static IFilterKeyPair ApplyFilter(this IFilterKeyPair filter,Enum enumfilter, IClientInfo client)
        {
            var actionFilter = Using<IDomainActionFilter>(enumfilter.ToString(), client);


                //GetServiceType<IDomainActionFilter>(enumfilter.ToString(),"Filters");
            filter.SetRef(actionFilter);
            
            return filter; 

        }


        public static T Using<T>(string filterName, IClientInfo client)
            where T : class, IFilter
        {
            string ClientPrefix = client.AssmPrefix;
            string DefaultPrefix = client.DefaultPrefix;
            string filter;

            if (client.AssmPrefix != client.DefaultPrefix)
                filter = string.Format("{0}.{1}.", ClientPrefix, EnumServiceTypes.Filters) + filterName;
            else
                filter = CommonUtil.GetResolveName(typeof(T), filterName);


            var exists = EngineContext
                       .Current
                       .ContainerManager.IsRegisteredByName(filter, typeof(T));

            if (!exists)
            {
                filter = string.Format("{0}.{1}.", DefaultPrefix, EnumServiceTypes.Filters) + filterName;
            }

            return GetServiceType<T>(filter);
        }


        public static IFilterKeyPair Using<TFilter>(Enum filter)
            where TFilter : class, IFilter
        {
            var actionFilter = GetServiceType<IDomainActionFilter>(filter.ToString());
            dynamic FilterInterface = GetServiceType<TFilter>(); 
            FilterInterface.SetRef(actionFilter);

            return FilterInterface;

        }


       public static IFilterKeyPair Using<TFilter>(Enum filter, IClientInfo clientInfo)
            where TFilter : class, IFilter
        {
            
            var actionFilter = Using<IDomainActionFilter>(filter.ToString(), clientInfo);
            var filterKeyPair = GetServiceType<TFilter>() as IFilterKeyPair;
            filterKeyPair.SetRef(actionFilter);

            return filterKeyPair;
        }

        public static IFilter Using(this IFilterFactory filter)
        {

            return default(IEditFilter);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="Handler"></param>
        /// <param name="EntityType"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static void SetPropValues<THandler>(this THandler Handler, DomainEntityType EntityType, string propName)
          where THandler : IServiceHandler
        {
            List<IEntity> resList = new List<IEntity>(((dynamic)Handler).Get());

            //foreach (var res in (((dynamic)Handler).Get()))
            //{
            //    resList.Add(res as IBaseEntity);
            //}

            resList.AsQueryable().SetPropValues(EntityType, propName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propValues"></param>
        /// <param name="Entity"></param>
        /// <param name="propName"></param>
        public static void SetPropValues<T>(this IQueryable<T> propValues
                                          ,DomainEntityType Entity
                                          ,string propName  //PropKeyPairFieldDef propKeyPair
                                          )
            where T : class,IEntity //    BaseEntity<T>
        {
            if (Entity.Attributes == null)
                Entity.Attributes = new ExpandoObject();

            var propList = new List<PropListKeyPair>();

            foreach (var fil in propValues)
            {
                var value = default(dynamic);

                var key = fil
                            .GetType()
                            .GetProperty("Id")
                            .GetValue(fil, null);

                var prop = fil
                            .GetType()
                            .GetProperty(propName);

                if (typeof(DomainEntityType).IsAssignableFrom(prop.PropertyType))
                {
                    value = ((dynamic)prop
                           .GetValue(fil, null)).Value;
                }
                else
                {
                    value = prop
                            .GetValue(fil, null);
                }

                propList.Add(
                    new PropListKeyPair() { Key = key.ToString()
                                          , Value = value }
                    
                    );
            }

            dynamic attr = new ExpandoObject();
            
            ((IDictionary<String, Object>)Entity.Attributes)["propList"] = propList;
            
        }


        public static bool SetValue(this IEnumerable<IFilterKeyPair> filterValues
                                    , DomainEntityType Entity)
        {
            if (filterValues == null)
                return false;

            var filterKeyPair = GetServiceType<IFilterKeyPair>() as IFilterKeyPair;
            var actionFilter = GetServiceType<IDomainActionFilter>();
            filterKeyPair.SetValue(Entity, actionFilter,filterValues);
            return true;
        }

        
        private static void GetResolveName(string fullName)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<IFilterKeyPair> Using<TFilter>(this IFilterFactory filter
                                                    , Enum KeyPairFilter
                                                    , IClientInfo clientInfo)
            where TFilter : IFilterKeyPair
        {

            string ClientPrefix = clientInfo.AssmPrefix;
            string DefaultPrefix = clientInfo.DefaultPrefix;
            string TfilterName = KeyPairFilter.ToString();
            string ResolvefilterName;



            if (clientInfo.AssmPrefix != clientInfo.DefaultPrefix)
                 ResolvefilterName = string.Format("{0}.{1}.", ClientPrefix, EnumServiceTypes.Filters) + TfilterName;
            else
                ResolvefilterName = CommonUtil.GetResolveName(typeof(IDomainActionFilter)
                                                             , Name: TfilterName);
            
            var exists = EngineContext
                       .Current
                       .ContainerManager.IsRegisteredByName(ResolvefilterName, typeof(IDomainActionFilter));

            if (!exists)
            {
                ResolvefilterName = string.Format("{0}.{1}.", DefaultPrefix, EnumServiceTypes.Filters) + TfilterName;
            }
            
            var actionFilter = GetServiceType<IDomainActionFilter>(ResolvefilterName);
            var filterKeyPair = GetServiceType<IFilterKeyPair>();
            if (actionFilter !=null)
                 filterKeyPair?.SetRef(actionFilter);

            return filterKeyPair?.GetFilterValues();
         
        }

        private static T GetFilterType<T>()
            where T : class, IFilterKeyPair
        {
            return GetServiceType<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sName"></param>
        /// <returns></returns>
        private static T GetServiceType<T>(string sName ="", EnumServiceTypes postFix = null)
            where T : class, IFilter
        {

            T handler = default(T);

            try
            {

                string typeNameResolve = typeof(T).Name.Substring(1);

                //check if type is register 
                // if not then resolve thru type name
                if (String.IsNullOrEmpty(sName))
                {
                    if (EngineContext
                           .Current
                           .ContainerManager
                           .IsRegistered(typeof(T)))
                    {
                        handler = HandlerResolve.Resolve<T>(AllowNull: true);
                    }
                }
                else
                {
                    typeNameResolve = CommonUtil.GetResolveName(typeof(T), sName, postFix);
                    handler = GetFilterByName<T>(typeNameResolve) as T;
                }


                if (handler == null)
                {
                    throw new NullReferenceException("Unable to resolve type with service locator; type " + typeof(T).Name);
                }

            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return handler as T;


        }

        public static T Using<T>(this IFilterFactory filter) 
            where T : class, IFilter
        {

            return GetServiceType<T>();
                               
        }


        private static T GetFilterByName<T>(string filterName = "")
            where T : class, IFilter
        {

            return EngineContext
                   .Current
                   .ContainerManager
                   .Resolve<T>(filterName) as T;
        }   


        public static void SetViewModelValues<T>(this IBaseService<T> Service,DomainViewModel View)
            where T : BaseEntity<T>
        {
            var entity = Service.CreateServiceEntity(CreateEntityOptions.EntityEdit);
            View.UpdateModel(entity);
        }

        private static void UpdateModel<V>(this V viewModel,IBaseEntity entity)
            where V : DomainViewModel
        {

            if (entity == null)
                return;

            var props = viewModel
                    .GetType()
                    .GetProperties()
                    .Where(w => w.PropertyType.IsSubclassOf(typeof(DomainEntityType)));

            foreach(var prop in props)
            {
                dynamic source = prop.GetValue(viewModel, null);
                var srcValue =
                        ((IDomainEntityType)
                        (entity.GetType()
                        .GetProperty(prop.Name)
                        .GetValue(entity, null))).Attributes;

                source.Attributes = srcValue;
                prop.SetValue(viewModel, source, null);

            }
        }
    }
}
