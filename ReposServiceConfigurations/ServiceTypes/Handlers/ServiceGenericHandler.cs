using Repos.DomainModel.Interface.Atrributes.DomainAttributes;
using Repos.DomainModel.Interface.Atrributes.ServiceAttributes;
using Repos.DomainModel.Interface.Filters;
using Repos.DomainModel.Interface.Interfaces;
using Repos.DomainModel.Interface.Interfaces.Filter;
using ReposCore.Caching;
using ReposCore.Infrastructure;
using ReposData.Repository;
using ReposServiceConfigurations.Common;
using ReposServiceConfigurations.ServiceTypes.Base;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ReposServiceConfigures.ServiceTypes.Handlers
{

    public abstract class ServiceGenericHandler<T>
        : BaseEntity<T>
        , IServiceHandler<T>
        , IEditFilter<T>
       , IDomainEntityHandler
       
        where T : BaseEntity<T>
    {
        readonly protected IRepository<T> _repos;
        private ICacheService _cache;

        public ICacheManager HandlerCache => _cache;


        public ServiceGenericHandler(){
        }

        public ServiceGenericHandler(IRepository<T> repos
                                    , ICacheService cache)
        {
            _repos = repos;
            _cache = cache;
        }
              

        public IQueryable ApplyFilter<TFilter>(T Entity) where TFilter : IEditFilter
        {
            dynamic Filter = GetFilter<TFilter>() as IEditFilter;
            return Filter.ApplyFilter(_repos.TableNoTracking);
        }

        public virtual List<T> Get()
        {
            var cacheKey = string.Concat("ServiceGenericHandler", "-", this.GetType().FullName);

            return HandlerCache.Get<List<T>>(cacheKey, () => 
            {
                return _repos.TableNoTracking.ToList();
            });

        }
            
        public IQueryable<T> Get<TFilter>()
            where TFilter : IEditFilter
        {
            dynamic Filter = GetFilter<TFilter>() as IEditFilter;
            return Filter.ApplyEditFilter(_repos.TableNoTracking);
        }

        public IFilter GetFilter<TFilter>()
            where TFilter : IEditFilter
        {
            return EngineContext
                        .Current
                        .ContainerManager
                        .Resolve(typeof(TFilter)) as IFilter;
                        
        }
    }

    [DomainNoBindAttribute, ServiceNoResolveAttribute]
    public sealed class GenericHandler
        : IServiceGenericHandler
    { 

        private IDbContext _context;
        public  GenericHandler(IDbContext context)
        {
            _context = context;
        }
        public IQueryable GetData<TDataEntity>()
                  where TDataEntity : IGenericHandler
        {

            var typeName = typeof(TDataEntity).Name.Substring(1);
            var t = typeof(TDataEntity);

            var typeNameResolve = CommonUtil.GetResolveName(t, typeName);


            dynamic oDomainType = EngineContext
                                    .Current
                                    .ContainerManager
                                    .Resolve<IGenericHandler>(typeNameResolve);

            var res = new List<TDataEntity>(oDomainType.Get());
         
            return res.AsQueryable();
        }

        
    }
     
}
