using Repos.DomainModel.Interface.Atrributes.DomainAttributes;
using Repos.DomainModel.Interface.Atrributes.ServiceAttributes;
using Repos.DomainModel.Interface.Filters;
using Repos.DomainModel.Interface.Interfaces;
using Repos.DomainModel.Interface.Interfaces.Filter;
using ReposCore.Infrastructure;
using ReposData.Repository;
using ReposServiceConfigurations.Common;
using System.Linq;

namespace ReposDomain.Handlers.Base
{

    public abstract class ServiceGenericHandler<T>
        : BaseEntity<T>
        , IServiceHandler<T>
        , IEditFilter<T>
        where T : BaseEntity<T>
    {
        readonly private IRepository<T> _repos;

      //  protected ServiceGenericHandler(){
      //  }

        public ServiceGenericHandler(IRepository<T> repos)
        {
            _repos = repos;
        }


        public IQueryable ApplyFilter<TFilter>(T Entity) where TFilter : IEditFilter
        {
            dynamic Filter = GetFilter<TFilter>() as IEditFilter;
            return Filter.ApplyFilter(_repos.TableNoTracking);
        }

        public virtual IQueryable<T> Get()
        {
            return  _repos.TableNoTracking;
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

            return oDomainType.Get();
        }

    }
     
}
