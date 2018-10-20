using Repos.DomainModel.Interface.Interfaces;
using ReposCore.Custom.Types;
using ReposCore.Extensions;
using ReposCore.FunctionInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ReposData.Repository
{
    public partial interface IRepository<T> where T : BaseEntity<T>
    {

        T Attach(T entity);
        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        T GetById(object id);

            
        /// <summary>
        /// Save entity
        /// </summary>
        /// <param name="entity">Entity</param>
        //Result Save();

        //Result Save(ModelStateDictionary modelState);
        //Result Save(ServiceRuleFunc<bool> ServiceRuleFunc
        //         , ModelStateDictionary modelState
        //         , object RuleFactory);

        Result Verify(ServiceRuleFunc<bool> ServiceRuleFunc
                    , ModelStateDictionary modelState
                    , object RuleFactory);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Add(T entity);

         /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Add(IEnumerable<T> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(T entity);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Result Update(IEnumerable<T> entities);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Result Delete(T entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Result Delete(IEnumerable<T> entities);

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> Table { get; }

        IQueryable<T> Where(Expression<Func<T, bool>> predicate );
        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }

        EntityRules ModifiedEntities { get; }
             
    }
}
