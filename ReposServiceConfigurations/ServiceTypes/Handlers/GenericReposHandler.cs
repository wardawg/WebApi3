using Repos.DomainModel.Interface.Filters;
using Repos.DomainModel.Interface.Interfaces;
using Repos.DomainModel.Interface.Interfaces.Filter;
using ReposCore.Infrastructure;
using ReposData.Repository;
using System.Linq;

namespace ReposServiceConfigures.ServiceTypes.Handlers
{
    public class GenericReposHandler<T>
        : IGenericHandler<T>
          , IEditFilter<T>
        where T : BaseEntity<T>
    {
        readonly private IRepository<T> _repos;

        public GenericReposHandler(IRepository<T> repos)
        {
            _repos = repos;
        }
        
        public IQueryable ApplyFilter<TFilter>(T Entity)
            where TFilter : IEditFilter
        {
            dynamic Filter = GetFilter<TFilter>() as IEditFilter;
            return Filter.ApplyFilter(_repos.TableNoTracking);
        }

        public IQueryable<T> Get()
        {
            return _repos.TableNoTracking;
        }

        public IQueryable<T> Get<TFilter>() where TFilter : IEditFilter
        {
            return _repos.TableNoTracking;
        }

        protected IFilter GetFilter<TFilter>() where TFilter : IEditFilter
        {
            return EngineContext
                        .Current
                        .ContainerManager
                        .ResolveUnregistered(typeof(TFilter)) as IFilter;
        }
        
    }
}
