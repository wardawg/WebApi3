using Repos.DomainModel.Interface.Filters;
using Repos.DomainModel.Interface.Interfaces.Filter;
using System.Collections.Generic;
using System.Linq;

namespace Repos.DomainModel.Interface.Interfaces
{
    public interface IServiceHandler
      : IHandler{
    }

    public interface IServiceHandler<T>
        : IServiceHandler
        where T : IBaseEntity
    {
        List<T> Get();
        IQueryable<T> Get<TFilter>()
            where TFilter : IEditFilter;
        IFilter GetFilter<TFilter>()
            where TFilter : IEditFilter;
    }
}
