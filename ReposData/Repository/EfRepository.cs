using Repos.DomainModel.Interface.Interfaces;
using ReposCore.Custom.Types;
using ReposCore.Extensions;
using ReposCore.FunctionInterfaces;
using ReposDomain.Extentions.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ReposData.Repository
{

    public partial class EfRepository<T> 
        : IRepository<T>  where T : BaseEntity<T>
    {
      #region Fields

        private readonly IDbContext _context;
        private IDbSet<T> _entities;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public EfRepository(IDbContext context)
        {
            this._context = context;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get full error
        /// </summary>
        /// <param name="exc">Exception</param>
        /// <returns>Error</returns>
        protected string GetFullErrorText(DbEntityValidationException exc)
        {
            var msg = string.Empty;
            foreach (var validationErrors in exc.EntityValidationErrors)
                foreach (var error in validationErrors.ValidationErrors)
                    msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
            return msg;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual T GetById(object id)
        {
            //see some suggested performance optimization (not tested)
            //http://stackoverflow.com/questions/11686225/dbset-find-method-ridiculously-slow-compared-to-singleordefault-on-id/11688189#comment34876113_11688189
            var result =  this.Entities.Find(id);

            result.SetCleanEntity();
            
            return result;
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Add(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                
                this.Entities.Add(entity);

              
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Add(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    this.Entities.Add(entity);
                
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

       
        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual Result Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");


                //  context.Entry(entity).State = EntityState.Modified;

                return Result.Success(); 
                        
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual Result Update(IEnumerable<T> entities)
        {
            try
            {
                 
                if (entities == null)
                    throw new ArgumentNullException("entities");

                return Result.Success();


            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual Result Delete(T entity)
        {
            try
            {
                if (entity == null)
                        throw new ArgumentNullException("entity");

                this.Entities.Remove(entity);
                return Result.Success();
               // return Save();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual Result Delete(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    this.Entities.Remove(entity);

                return Result.Success();

            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        #endregion

        #region Properties

        IQueryable<T> IRepository<T>.Where(Expression<Func<T, bool>> predicate)
        {
            var res = this._context.Set<T>().Where(predicate);
            return res;
        }

         /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                var result = this.Entities;
                result.SetCleanEntity();
                return result;
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }

        /// <summary>
        /// Entities
        /// </summary>
        protected virtual IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }   
        }

       public virtual EntityRules ModifiedEntities
        {
            get
            {
                EntityRules Entities = new EntityRules(null);


                int i = 0;

                Func<int> oFunc = () => { return i++; };


                _context.Tracker()
                  .Entries()
                  .Where(s => s.State == EntityState.Added
                                      || s.State == EntityState.Modified
                                      || s.State == EntityState.Deleted)
                  .ToList()
                  .ForEach(e => Entities.Rules.Add(
                      string.Format("{0}_{1}", oFunc.Invoke(), ((IBaseEntity)e.Entity).ObjName)
                      , (IBaseEntity)e.Entity));
                                
                return Entities;
            }
            
        }

        public virtual IEnumerable<T> Get(
               List<Expression<Func<T, bool>>> filter,
               Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
               int? Page = 0,
               params Expression<Func<T, object>>[] included)
        {

            IQueryable<T> query = _entities;

            foreach (var z in included)
            {
                query = query.Include(z);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
                query = query.Skip((Page.Value - 1) * 30).Take(30);
            }
            if (filter != null)
            {
                foreach (var z in filter)
                {
                    query = query.Where(z);
                }
            }
            return query.ToList();
        }

        
        void IRepository<T>.Update(T entity)
        {
        
            var e = Entities.Find(entity.Id);

            if (e == null)
            {
                return;
            }

            _context.Entity(e).CurrentValues.SetValues(entity);
            _context.Entity(e).State = EntityState.Modified;
            
        }
                          
        public virtual bool RunRules(ServiceRuleFunc<bool> RulesFunc, ModelStateDictionary modelState,object RuleFactory)
        {
            return RulesFunc.ExecRules(ModifiedEntities, modelState, RuleFactory);
        }

      

        public virtual Result Verify(ServiceRuleFunc<bool> RulesFunc
                                    , ModelStateDictionary modelState
                                    , object RuleFactory)
        {
            bool ret = true;

            if (RulesFunc != null)
                ret = RunRules(RulesFunc, modelState, RuleFactory);

            return ret ? Result.Success() : Result.Fail(string.Empty);
            
        }

        public T Attach(T entity)
        {
            return this.Entities.Attach(entity);
        }
        
        #endregion
    }
}
