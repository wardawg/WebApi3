using ProjectDependResolver;
using Repos.DomainModel.Interface;
using Repos.DomainModel.Interface.Atrributes.DomainAttributes;
using Repos.DomainModel.Interface.Interfaces;
using ReposCore.Extensions;
using ReposCore.Infrastructure;
using ReposData.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace ReposData.Repository
{
    
    public class ReposContext 
        : DbContext, IDbContext 
    {


        private string contextName;
        #region Ctor

        public ReposContext(string nameOrConnectionString)
              : base(nameOrConnectionString)
           
        {
            contextName = nameOrConnectionString;
            Database.SetInitializer<ReposContext>(null);
            SetContextOptions();
    
        }

        public ReposContext(){
        }

        protected virtual void SetContextOptions()
        {
            base.Configuration.AutoDetectChangesEnabled = false;
            base.Configuration.ProxyCreationEnabled = false;
            
        }

        public bool AutoDetectChangesEnabled
        {
            get
            {
                return base.Configuration.AutoDetectChangesEnabled;
            }

            set
            {
             //  base.Configuration.AutoDetectChangesEnabled = false;
                //throw new NotImplementedException();
            }
        }

        public bool ProxyCreationEnabled
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        bool IDbContext.ProxyCreationEnabled
        {
            get
            {
                return false;
            }

            set
            {
                
            }
        }

        bool IDbContext.AutoDetectChangesEnabled
        {
            get
            {
                return true;
                //throw new NotImplementedException();
            }

            set
            {
                
                throw new NotImplementedException();
            }
        }

        public void Detach(object entity)
        {
            throw new NotImplementedException();
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = default(int?), params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity<TEntity>, new()
        {
            //add parameters to command
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }

            var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();
                        
            bool acd = this.Configuration.AutoDetectChangesEnabled;
            try
            {
                this.Configuration.AutoDetectChangesEnabled = false;

                for (int i = 0; i < result.Count; i++)
                    result[i] = AttachEntityToContext(result[i]);
            }
            finally
            {
                this.Configuration.AutoDetectChangesEnabled = acd;
            }

            return result;
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            
             throw new NotImplementedException();
        }

        IDbSet<TEntity> IDbContext.Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns>int</returns>
        protected int GetTypeInheritsNumber(Type t)
        {
            Type tempType = t;
            int typeOrder = 0;

            do

               
                if (tempType.BaseType != null && tempType.BaseType != typeof(Object))
                {
                    typeOrder++;
                    if (tempType.BaseType.IsGenericType)
                        break;
                    else
                        tempType = tempType.BaseType;
                }
                else
                    break;

            while (true);

            return typeOrder;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected virtual void MapModelOverride(DbModelBuilder modelBuilder)
        {


            var maps = new IEnumerable<EntityMapType>[]
                          {   GetMap<IDomainEntityHandler>()
                             ,GetMap<IReposEntityType>()
                          }.SelectMany(s=> s);

            DoBuild(maps, modelBuilder);
         
        }

        protected virtual void DoBuild(IEnumerable<EntityMapType> map, DbModelBuilder modelBuilder)
        {
            var DomainEntities
                = new GetDbHelperTablesPrimKeyCol(Database.Connection
                                                  , contextName
                                                  , map
                                                    .Select(s => s)
                                                    .Distinct()
                                                    .ToList()
                                                  );

            Type generic = typeof(ReposEntityHelperTypeConfiguration<>);

            List<Type> mapType = new List<Type>();

            foreach (var entity in DomainEntities
                                    .GetEntityMapping()
                                    .OrderBy(o=> o.MappingOrder))
            {
                var t = default(Type);

                if (entity.EntityType.BaseType != null)
                    t = entity.EntityType.BaseType.GetGenericArguments().FirstOrDefault();
                else
                    t = entity.EntityType;

               // entity.EntityType.BaseType.GetGenericArguments()

                if (mapType.Contains(t))
                    continue;

                mapType.Add(t);

                var parms = new object[] { };
                var tblName = string.Empty;
                var pkName = string.Empty;
                Type constructed = entity.EntityType;

                if ((entity.PkMapping != null
                        && (entity.EntityType.GetConstructors().Length > 1
                            || entity.MappingTypeName == "IDomainEntityHandler")))
                {
                    tblName = entity.PkMapping.TableName;
                    pkName = entity.PkMapping.PkColumnName;
                }


                switch (entity.MappingTypeName)
                {
                    case "IReposEntityType":
                        if (!String.IsNullOrEmpty(tblName) && !String.IsNullOrEmpty(pkName))
                            parms = new object[] { tblName, pkName };
                        break;

                    case "IDomainEntityHandler":
                        if (String.IsNullOrEmpty(pkName))
                            throw new
                             Exception(string.Format("Entity {0} Must Exists/Contain A Primary Key", entity.EntityName));

                        Type[] typeArgs = { entity.EntityType };
                        constructed = generic.MakeGenericType(typeArgs);


                        parms = new object[] { pkName };
                        break;
                }

                
                if (parms.Length != 0)
                    modelBuilder
                        .Configurations
                        .Add((dynamic)Activator
                        .CreateInstance(constructed, parms));

                else
                    modelBuilder
                        .Configurations
                        .Add((dynamic)Activator
                        .CreateInstance(constructed));

            }
        }


        protected virtual IEnumerable<EntityMapType> GetMap<T>()
        {

            var interfaceName = typeof(T).Name;

            var maps = EngineContext
                                  .Current
                                  .ContainerManager
                                  .Resolve<ITypeFinder>()
                                  .FindClassesOfType<T>(false)
                                  .Where(w => !w.GetCustomAttributes(typeof(DomainNoBindAttribute), true)
                                         .Any() && !w.ContainsGenericParameters)
                                  .Select(s => new EntityMapType
                                  {
                                      EntityTypeName = interfaceName
                                      ,EntityType = GetGenericInterfaceType<T>(s)
                                      ,InheritNumber = GetTypeInheritsNumber(s)
                                  });


            var EntityTypes = maps
                               .Where(w => w.InheritNumber ==
                                       maps
                                          .Where(w2 => w2.EntityType.Name == w.EntityType.Name)
                                          .Min(m => m.InheritNumber)); // base types only

            return EntityTypes;
        }

        protected virtual Type GetGenericInterfaceType<T>(Type t)
        {
            Type ret = t;

            if (t.GetInterfaces().Any())
            {
                try
                {

                    ret = t
                            .GetInterfaces()
                            .Where(w => typeof(IBaseEntity).IsAssignableFrom(w) && w.IsGenericType)
                            .FirstOrDefault()?
                            .GenericTypeArguments?
                            .FirstOrDefault() ?? t;
                }
                catch
                {
                    System.Diagnostics.Debug.Write("");
                }
            }

            return ret;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {


            MapModelOverride(modelBuilder);

            
            modelBuilder
                .Conventions
                .Add(new NonPublicColumnAttributeConvention());

            //modelBuilder.Conventions
            //      .Remove<PluralizingTableNameConvention>();
                       
            base.OnModelCreating(modelBuilder);

        }
        
        Result IDbContext.SaveChanges()
        {
            
            return base.SaveChanges() != 0 ? Result.Success() : Result.Fail(string.Empty);
        }
    

       Result IDbContext.SaveChanges(ModelStateDictionary modelState)
        {
            try
            {
                
                return base.SaveChanges() != 0 ? Result.Success() : Result.Fail(string.Empty);
            }
            catch(DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                foreach(var err in errorMessages)
                {
                    modelState.AddModelError("", err);
                }

                return Result.Fail(string.Empty);
                

            }
            
        }

            

        IList<TEntity> IDbContext.ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
        {
            return new List<TEntity>();
                    
        }

        IEnumerable<TElement> IDbContext.SqlQuery<TElement>(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        int IDbContext.ExecuteSqlCommand(string sql, bool doNotEnsureTransaction, int? timeout, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        void IDbContext.Detach(object entity)
        {
            
           
            throw new NotImplementedException();
        }

        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity<TEntity>, new()
        {
            //little hack here until Entity Framework really supports stored procedures
            //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                //attach new entity
                Set<TEntity>().Attach(entity);
                return entity;
            }

            //entity is already loaded
            return alreadyAttached;
        }

        public DbChangeTracker Tracker()
        {
           return base.ChangeTracker;
        }
                
        public DbEntityEntry Entity<TEntity>(TEntity entity) where TEntity : BaseEntity<TEntity>
        {
            return Entry(entity);
        }

        public void Attach<TEntity>(TEntity entity)
        {
           // return entity.AttachEntityToContext<TEntity>(entity);
        }
    }

}
