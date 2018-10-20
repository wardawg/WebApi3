using Repos.DomainModel.Interface.Filters;
using Repos.DomainModel.Interface.Interfaces.Filter;
using System.Linq;

namespace Repos.DomainModel.Interface.Interfaces
{
    public interface IGenericHandler 
        : IDomainEntityHandler{
    }

    public interface IGenericHandler<T> 
        : IGenericHandler
    {
        IQueryable<T> Get();

        IQueryable<T> Get<TFilter>()
            where TFilter : IEditFilter;

       // IFilter GetFilter<TFilter>()
         //   where TFilter : IFilter;
    }
}
